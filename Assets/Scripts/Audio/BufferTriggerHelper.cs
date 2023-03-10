using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using UnityEngine;

//https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types

//TODO: alter based on define
using Float = System.Double;
using MillisecondTime = System.Double;
using ParameterValue = System.Double;
using ParameterIndex = System.UIntPtr;
using MessageTag = System.UInt32;

public class BufferTriggerHelper : MonoBehaviour {
    public const byte NOTE_OFF = 0x80;
    public const byte NOTE_ON = 0x90;
    public const byte KEY_PRESSURE = 0xA0;
    public const byte CONTROL_CHANGE = 0xB0;
    public const byte PITCH_BEND_CHANGE = 0xE0;
    public const byte SONG_POSITION_POINTER = 0xF2;
    public const byte PROGRAM_CHANGE = 0xC0;
    public const byte CHANNEL_PRESSURE = 0xD0;
    public const byte QUARTER_FRAME = 0xF1;
    public const byte SONG_SELECT = 0xF3;
    public const byte TUNE_REQUEST = 0xF6;
    public const byte TIMING_CLOCK = 0xF8;
    public const byte START = 0xFA;
    public const byte CONTINUE = 0xFB;
    public const byte STOP = 0xFC;
    public const byte ACTIVE_SENSE = 0xFE;
    public const byte RESET = 0xFF;
    public const byte SYSEX_START = 0xF0;
    public const byte SYSEX_END = 0xF7;

    public enum MessageEventType {
        Number,
        List,
        Bang
    }

    // Unity doesn't support JsonDocument (yet) so we work around it by specifying our own object with the keys we care about deserializing
    [System.Serializable]
    public class ParameterInfo {
        public int index;

        public string name;
        public string paramId;
        public string displayName;
        public string unit;

        public Float minimum;
        public Float maximum;
        public Float initialValue;
        public int steps;

        public List<string> enumValues;
        public string meta;
    }

    [System.Serializable]
    public class Port {
        public string tag;
        public string meta;
    }

    [System.Serializable]
    public class DataRef {
        public string id;
        public string type;
        public string file;
    }


    [System.Serializable]
    private class PatcherDescription {
        public int numParameters;
        public List<ParameterInfo> parameters;
        public List<Port> inports;
        public List<Port> outports;
        public List<DataRef> externalDataRefs;
    }

    public class ParameterChangedEventArgs : EventArgs {
        public ParameterChangedEventArgs(int index, ParameterValue value, MillisecondTime time)
        {
            Index = index;
            Value = value;
            Time = time;
        }

        public int Index { get; private set; }
        public ParameterValue Value { get; private set; }
        public MillisecondTime Time { get; private set; }

    }

    public class MessageEventArgs : EventArgs {
        public MessageEventArgs(MessageTag tag, MessageEventType messageType, Float[] values, MillisecondTime time)
        {
            Tag = tag;
            Type = messageType;
            Values = values;
            Time = time;
        }

        public MessageTag Tag { get; private set; }
        public MessageEventType Type { get; private set; }
        public Float[] Values { get; private set; }
        public MillisecondTime Time { get; private set; }

    }
    public event EventHandler<ParameterChangedEventArgs> ParameterChangedEvent;
    public event EventHandler<MessageEventArgs> MessageEvent;

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void ReleaseDataRefDelegate(UIntPtr id);

    [DllImport("BufferTrigger")]
    private static extern MessageTag RNBOTag(IntPtr tagString);

    [DllImport("BufferTrigger")]
    private static extern IntPtr RNBOGetDescription();

    [DllImport("BufferTrigger")]
    private static extern bool RNBOResolveTag(int key, MessageTag tag, out IntPtr tagStr);

    [DllImport("BufferTrigger")]
    private static extern bool RNBOPoll(int key);

    [DllImport("BufferTrigger")]
    private static extern bool RNBOSetParamValue(int key, ParameterIndex index, ParameterValue value, MillisecondTime attime);

    [DllImport("BufferTrigger")]
    private static extern bool RNBOGetParamValue(int key, ParameterIndex index, out ParameterValue value);

    [DllImport("BufferTrigger")]
    private static extern bool RNBOSetParamValueNormalized(int key, ParameterIndex index, ParameterValue value, MillisecondTime attime);

    [DllImport("BufferTrigger")]
    private static extern bool RNBOGetParamValueNormalized(int key, ParameterIndex index, out ParameterValue value);

    [DllImport("BufferTrigger", CallingConvention = CallingConvention.StdCall)]
    public static extern bool RNBORegisterParameterEventCallback(int key, IntPtr callback);

    [DllImport("BufferTrigger", CallingConvention = CallingConvention.StdCall)]
    public static extern bool RNBORegisterMessageEventCallback(int key, IntPtr callback);

    [DllImport("BufferTrigger")]
    public static extern bool RNBOSendMessageBang(int key, MessageTag tag, MillisecondTime atTime);

    [DllImport("BufferTrigger")]
    public static extern bool RNBOSendMessageNumber(int key, MessageTag tag, Float value, MillisecondTime atTime);

    [DllImport("BufferTrigger")]
    public static extern bool RNBOSendMessageList(int key, MessageTag tag, [MarshalAs(UnmanagedType.LPArray)] Float[] list, IntPtr listlen, MillisecondTime atTime);

    [DllImport("BufferTrigger")]
    public static extern bool RNBOSendMIDI(int key, [MarshalAs(UnmanagedType.LPArray)] byte[] data, IntPtr dataLen, MillisecondTime atTime);

    [DllImport("BufferTrigger")]
    public static extern bool RNBOCopyLoadDataRef(int key, IntPtr id, [MarshalAs(UnmanagedType.LPArray)] System.Single[] data, IntPtr datalen, IntPtr channels, IntPtr samplerate);

    [DllImport("BufferTrigger")]
    public static extern bool RNBOReleaseDataRef(int key, IntPtr id);

    public ParameterInfo[] Parameters {
        get; private set;
    }

    public Port[] Inports {
        get; private set;
    }

    public Port[] Outports {
        get; private set;
    }

    public DataRef[] DataRefs {
        get; private set;
    }

    public static MessageTag Tag(string v) {
        IntPtr tagPtr = (IntPtr)Marshal.StringToHGlobalAnsi(v);
        var r = RNBOTag(tagPtr);
        Marshal.FreeHGlobal(tagPtr);
        return r;
    }

    private static Dictionary<int, GameObject> instances = new Dictionary<int, GameObject>();
    public static BufferTriggerHelper FindById(int key) {
        lock (instances) {
            GameObject go;
            if (!instances.TryGetValue(key, out go)) {
                go = new GameObject(String.Format("BufferTriggerHelperObject{0}", key));
                DontDestroyOnLoad(go);
                instances.Add(key, go);

                BufferTriggerHelper helper = go.AddComponent<BufferTriggerHelper>();
                helper.PluginKey = key;
                return helper;
            }
            return go.GetComponent<BufferTriggerHelper>();
        }
    }

    private int pluginKey;
    public int PluginKey {
        get => pluginKey;
        internal set {
            pluginKey = value;
            string descString = Marshal.PtrToStringAnsi(RNBOGetDescription());

            PatcherDescription desc = JsonUtility.FromJson<PatcherDescription>(descString)!;
            Parameters = desc.parameters.ToArray();
            Inports = desc.inports.ToArray();
            Outports = desc.outports.ToArray();
            DataRefs = desc.externalDataRefs.ToArray();
        }
    }

    private bool registered = false;
    void Update() {
        if (!registered) {
            registered = RegisterParameterEventDelegate() && RegisterMessageEventDelegate();
        }
        RNBOPoll(pluginKey);
    }

    public bool ResolveTag(MessageTag tag, out string tagStr) {
        IntPtr p;
        var r = RNBOResolveTag(pluginKey, tag, out p);
        if (r) {
            tagStr = Marshal.PtrToStringAnsi(p);
        } else {
            tagStr = string.Empty;
        }
        return r;
    }

    public ParameterInfo GetParamByIndex(int index) => Array.Find(Parameters, p => p.index == index);
    public ParameterInfo GetParamByName(string name) => Array.Find(Parameters, p => name.Equals(p.name));
    public ParameterInfo GetParamById(string id) => Array.Find(Parameters, p => id.Equals(p.paramId));

    private Dictionary<string, int> parameterNameToIndex;
    public int? GetParamIndexByName(string name) {
        if (parameterNameToIndex == null) {
            parameterNameToIndex = new Dictionary<string, int>();
            foreach (var p in Parameters) {
                parameterNameToIndex.Add(p.name, p.index);
            }
        }
        int index = 0;
        if (parameterNameToIndex.TryGetValue(name, out index)) {
            return index;
        }
        return null;
    }

    private Dictionary<string, int> parameterIdToIndex;
    public int? GetParamIndexById(string id) {
        if (parameterIdToIndex == null) {
            parameterIdToIndex = new Dictionary<string, int>();
            foreach (var p in Parameters) {
                parameterIdToIndex.Add(p.name, p.index);
            }
        }
        int index = 0;
        if (parameterIdToIndex.TryGetValue(id, out index)) {
            return index;
        }
        return null;
    }

    public bool GetParamValue(int index, out ParameterValue value) {
        return RNBOGetParamValue(pluginKey, (ParameterIndex)index, out value);
    }

    public bool SetParamValue(int index, ParameterValue value, MillisecondTime attime = 0) {
        return RNBOSetParamValue(pluginKey, (ParameterIndex)index, value, attime);
    }

    public bool GetParamValueNormalized(int index, out ParameterValue value) {
        return RNBOGetParamValueNormalized(pluginKey, (ParameterIndex)index, out value);
    }

    public bool SetParamValueNormalized(int index, ParameterValue value, MillisecondTime attime = 0) {
        return RNBOSetParamValueNormalized(pluginKey, (ParameterIndex)index, value, attime);
    }

    public bool SendBang(MessageTag tag, MillisecondTime atTime = 0) {
        return RNBOSendMessageBang(pluginKey, tag, atTime);
    }

    public bool SendMessage(MessageTag tag, Float value, MillisecondTime atTime = 0) {
        return RNBOSendMessageNumber(pluginKey, tag, value, atTime);
    }

    public bool SendMessage(MessageTag tag, Float[] values, MillisecondTime atTime = 0) {
        return RNBOSendMessageList(pluginKey, tag, values, (IntPtr)values.Length, atTime);
    }

    public bool SendMIDI(byte[] data, MillisecondTime atTime = 0) {
        return RNBOSendMIDI(pluginKey, data, (IntPtr)data.Length, atTime);
    }

    public bool SendMIDINoteOn(byte channel, byte noteNum, byte velocity, MillisecondTime atTime = 0) {
        Debug.Assert(channel < (byte)16);
        Debug.Assert(noteNum < (byte)128);
        Debug.Assert(velocity < (byte)128);
        byte[] data = new byte[] { (byte)(channel | NOTE_ON), noteNum, velocity };
        return SendMIDI(data, atTime);
    }

    public bool SendMIDINoteOff(byte channel, byte noteNum, byte velocity = (byte)0, MillisecondTime atTime = 0) {
        Debug.Assert(channel < (byte)16);
        Debug.Assert(noteNum < (byte)128);
        Debug.Assert(velocity < (byte)128);
        byte[] data = new byte[] { (byte)(channel | NOTE_OFF), noteNum, velocity };
        return SendMIDI(data, atTime);
    }

    public bool SendMIDICC(byte channel, byte num, byte val, MillisecondTime atTime = 0) {
        Debug.Assert(channel < (byte)16);
        Debug.Assert(num < (byte)128);
        Debug.Assert(val < (byte)128);
        byte[] data = new byte[] { (byte)(channel | CONTROL_CHANGE), num, val };
        return SendMIDI(data, atTime);
    }

    public bool LoadDataRef(string id, float[] data, int channels, int samplerate) {
        IntPtr idPtr = (IntPtr)Marshal.StringToHGlobalAnsi(id);
        var r = RNBOCopyLoadDataRef(pluginKey, idPtr, data, (IntPtr)data.Length, (IntPtr)channels, (IntPtr)samplerate);
        Marshal.FreeHGlobal(idPtr);
        return r;
    }

    public bool ReleaseDataRef(string id) {
        IntPtr idPtr = (IntPtr)Marshal.StringToHGlobalAnsi(id);
        var r = RNBOReleaseDataRef(pluginKey, idPtr);
        Marshal.FreeHGlobal(idPtr);
        return r;
    }

    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void InternalParameterEventDelegate(ParameterIndex index, ParameterValue value, MillisecondTime time);

    //keep a reference to the callback so that it isn't garabage collected
    private InternalParameterEventDelegate registeredParameterEventDelegate;
    private bool RegisterParameterEventDelegate() {
        InternalParameterEventDelegate d = (index, value, time) => {
            var e = ParameterChangedEvent;
            if (e != null) {
                //cast to int as it is easier to use for unity
                e(this, new ParameterChangedEventArgs((int)index, value, time));
            }
        };
        var r = RNBORegisterParameterEventCallback(pluginKey, Marshal.GetFunctionPointerForDelegate(d));
        registeredParameterEventDelegate = d;
        return r;
    }


    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    private delegate void InternalMessageEventDelegate(MessageTag tag, IntPtr messageType, IntPtr valuesPtr, IntPtr valueCount, MillisecondTime time);

    //keep a reference to the callback so that it isn't garabage collected
    private InternalMessageEventDelegate registeredMessageEventDelegate;
    private bool RegisterMessageEventDelegate() {
        InternalMessageEventDelegate d = (tag, messageType, valuesPtr, valueCount, time) => {
            var e = MessageEvent;
            if (e != null) {
                MessageEventType t = MessageEventType.Bang;
                Float[] values = new Float[(int)valueCount];

                if (messageType.Equals(IntPtr.Zero)) {
                    Marshal.Copy(valuesPtr, values, 0, 1);
                    t = MessageEventType.Number;
                } else if (messageType.Equals((IntPtr)1)) {
                    Marshal.Copy(valuesPtr, values, 0, (int)valueCount);
                    t = MessageEventType.List;
                }

                e(this, new MessageEventArgs(tag, t, values, time));
            }
        };
        var r = RNBORegisterMessageEventCallback(pluginKey, Marshal.GetFunctionPointerForDelegate(d));
        registeredMessageEventDelegate = d;
        return r;
    }
}
