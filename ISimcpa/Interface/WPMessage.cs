//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: WPMessage.proto
namespace winprint
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Header")]
  public partial class Header : global::ProtoBuf.IExtensible
  {
    public Header() {}
    
    private winprint.Header.Command _cmd;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"cmd", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public winprint.Header.Command cmd
    {
      get { return _cmd; }
      set { _cmd = value; }
    }
    private int _seq;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"seq", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int seq
    {
      get { return _seq; }
      set { _seq = value; }
    }
    private int _restaurant_id;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"restaurant_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int restaurant_id
    {
      get { return _restaurant_id; }
      set { _restaurant_id = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"Command")]
    public enum Command
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"HEARTBEAT", Value=1)]
      HEARTBEAT = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PRINTERTASK", Value=2)]
      PRINTERTASK = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"PRINTERINFO", Value=3)]
      PRINTERINFO = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"NOTIFY", Value=4)]
      NOTIFY = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UPDATECONFIG", Value=5)]
      UPDATECONFIG = 5
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ServerConfig")]
  public partial class ServerConfig : global::ProtoBuf.IExtensible
  {
    public ServerConfig() {}
    
    private string _host;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"host", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string host
    {
      get { return _host; }
      set { _host = value; }
    }
    private int _port;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"port", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int port
    {
      get { return _port; }
      set { _port = value; }
    }
    private long _restaurant_id;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"restaurant_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long restaurant_id
    {
      get { return _restaurant_id; }
      set { _restaurant_id = value; }
    }
    private int _heartbeat_interval;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"heartbeat_interval", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int heartbeat_interval
    {
      get { return _heartbeat_interval; }
      set { _heartbeat_interval = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Result")]
  public partial class Result : global::ProtoBuf.IExtensible
  {
    public Result() {}
    
    private winprint.Result.Code _code;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"code", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public winprint.Result.Code code
    {
      get { return _code; }
      set { _code = value; }
    }
    private string _message = "";
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"message", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string message
    {
      get { return _message; }
      set { _message = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"Code")]
    public enum Code
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"SUCC", Value=0)]
      SUCC = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ERR", Value=1)]
      ERR = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"UNKNOWN", Value=2)]
      UNKNOWN = 2
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"HeartBeat")]
  public partial class HeartBeat : global::ProtoBuf.IExtensible
  {
    public HeartBeat() {}
    
    private int _printer_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"printer_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int printer_id
    {
      get { return _printer_id; }
      set { _printer_id = value; }
    }
    private winprint.HeartBeat.Status _status;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public winprint.HeartBeat.Status status
    {
      get { return _status; }
      set { _status = value; }
    }
    private int _queue;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"queue", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int queue
    {
      get { return _queue; }
      set { _queue = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"Status")]
    public enum Status
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"OFFLINE", Value=0)]
      OFFLINE = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LESSPAPER", Value=1)]
      LESSPAPER = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ONLINE", Value=2)]
      ONLINE = 2
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"SlaveFoods")]
  public partial class SlaveFoods : global::ProtoBuf.IExtensible
  {
    public SlaveFoods() {}
    
    private string _name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _price;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"price", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string price
    {
      get { return _price; }
      set { _price = value; }
    }
    private int _num;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Foods")]
  public partial class Foods : global::ProtoBuf.IExtensible
  {
    public Foods() {}
    
    private string _name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _price;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"price", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string price
    {
      get { return _price; }
      set { _price = value; }
    }
    private int _num;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int num
    {
      get { return _num; }
      set { _num = value; }
    }
    private readonly global::System.Collections.Generic.List<winprint.SlaveFoods> _slavefoods = new global::System.Collections.Generic.List<winprint.SlaveFoods>();
    [global::ProtoBuf.ProtoMember(4, Name=@"slavefoods", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<winprint.SlaveFoods> slavefoods
    {
      get { return _slavefoods; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"BookHead")]
  public partial class BookHead : global::ProtoBuf.IExtensible
  {
    public BookHead() {}
    
    private string _arrival_at;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"arrival_at", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string arrival_at
    {
      get { return _arrival_at; }
      set { _arrival_at = value; }
    }
    private string _people;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"people", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string people
    {
      get { return _people; }
      set { _people = value; }
    }
    private string _phone;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"phone", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string phone
    {
      get { return _phone; }
      set { _phone = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Head")]
  public partial class Head : global::ProtoBuf.IExtensible
  {
    public Head() {}
    
    private string _title;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"title", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string title
    {
      get { return _title; }
      set { _title = value; }
    }
    private string _subtitle;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"subtitle", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string subtitle
    {
      get { return _subtitle; }
      set { _subtitle = value; }
    }
    private string _way;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"way", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string way
    {
      get { return _way; }
      set { _way = value; }
    }
    private string _password = "";
    [global::ProtoBuf.ProtoMember(4, IsRequired = false, Name=@"password", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string password
    {
      get { return _password; }
      set { _password = value; }
    }
    private winprint.BookHead _bookhead = null;
    [global::ProtoBuf.ProtoMember(5, IsRequired = false, Name=@"bookhead", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public winprint.BookHead bookhead
    {
      get { return _bookhead; }
      set { _bookhead = value; }
    }
    private string _table_card_no = "";
    [global::ProtoBuf.ProtoMember(6, IsRequired = false, Name=@"table_card_no", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string table_card_no
    {
      get { return _table_card_no; }
      set { _table_card_no = value; }
    }
    private readonly global::System.Collections.Generic.List<string> _moreitems = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(7, Name=@"moreitems", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> moreitems
    {
      get { return _moreitems; }
    }
  
    private string _note = "";
    [global::ProtoBuf.ProtoMember(8, IsRequired = false, Name=@"note", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue("")]
    public string note
    {
      get { return _note; }
      set { _note = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Body")]
  public partial class Body : global::ProtoBuf.IExtensible
  {
    public Body() {}
    
    private readonly global::System.Collections.Generic.List<winprint.Foods> _foods = new global::System.Collections.Generic.List<winprint.Foods>();
    [global::ProtoBuf.ProtoMember(1, Name=@"foods", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<winprint.Foods> foods
    {
      get { return _foods; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Tail")]
  public partial class Tail : global::ProtoBuf.IExtensible
  {
    public Tail() {}
    
    private string _restaurant_name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"restaurant_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string restaurant_name
    {
      get { return _restaurant_name; }
      set { _restaurant_name = value; }
    }
    private string _totalprice;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"totalprice", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string totalprice
    {
      get { return _totalprice; }
      set { _totalprice = value; }
    }
    private string _orderno;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"orderno", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string orderno
    {
      get { return _orderno; }
      set { _orderno = value; }
    }
    private string _phone;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"phone", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string phone
    {
      get { return _phone; }
      set { _phone = value; }
    }
    private string _time;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"time", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string time
    {
      get { return _time; }
      set { _time = value; }
    }
    private readonly global::System.Collections.Generic.List<string> _moreitems = new global::System.Collections.Generic.List<string>();
    [global::ProtoBuf.ProtoMember(6, Name=@"moreitems", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<string> moreitems
    {
      get { return _moreitems; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Receipt")]
  public partial class Receipt : global::ProtoBuf.IExtensible
  {
    public Receipt() {}
    
    private int _printer_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"printer_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int printer_id
    {
      get { return _printer_id; }
      set { _printer_id = value; }
    }
    private long _order_id;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"order_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long order_id
    {
      get { return _order_id; }
      set { _order_id = value; }
    }
    private int _print_num;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"print_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int print_num
    {
      get { return _print_num; }
      set { _print_num = value; }
    }
    private winprint.Head _head;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"head", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public winprint.Head head
    {
      get { return _head; }
      set { _head = value; }
    }
    private winprint.Body _body;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"body", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public winprint.Body body
    {
      get { return _body; }
      set { _body = value; }
    }
    private winprint.Tail _tail;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"tail", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public winprint.Tail tail
    {
      get { return _tail; }
      set { _tail = value; }
    }
    private int _is_force_print;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"is_force_print", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int is_force_print
    {
      get { return _is_force_print; }
      set { _is_force_print = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"ImageReceipt")]
  public partial class ImageReceipt : global::ProtoBuf.IExtensible
  {
    public ImageReceipt() {}
    
    private int _printer_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"printer_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int printer_id
    {
      get { return _printer_id; }
      set { _printer_id = value; }
    }
    private int _print_num;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"print_num", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int print_num
    {
      get { return _print_num; }
      set { _print_num = value; }
    }
    private int _img_data_size;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"img_data_size", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int img_data_size
    {
      get { return _img_data_size; }
      set { _img_data_size = value; }
    }
    private byte[] _img_content;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"img_content", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public byte[] img_content
    {
      get { return _img_content; }
      set { _img_content = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"NotifyOrders")]
  public partial class NotifyOrders : global::ProtoBuf.IExtensible
  {
    public NotifyOrders() {}
    
    private long _order_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"order_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long order_id
    {
      get { return _order_id; }
      set { _order_id = value; }
    }
    private winprint.NotifyOrders.StatusCode _status;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public winprint.NotifyOrders.StatusCode status
    {
      get { return _status; }
      set { _status = value; }
    }
    private readonly global::System.Collections.Generic.List<int> _err_list = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(3, Name=@"err_list", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> err_list
    {
      get { return _err_list; }
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"StatusCode")]
    public enum StatusCode
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"OK", Value=0)]
      OK = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ERR", Value=1)]
      ERR = 1
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Printer")]
  public partial class Printer : global::ProtoBuf.IExtensible
  {
    public Printer() {}
    
    private int _id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int id
    {
      get { return _id; }
      set { _id = value; }
    }
    private winprint.Printer.Status _status;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"status", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public winprint.Printer.Status status
    {
      get { return _status; }
      set { _status = value; }
    }
    private winprint.Printer.PrinterType _type;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public winprint.Printer.PrinterType type
    {
      get { return _type; }
      set { _type = value; }
    }
    private winprint.Printer.InterfaceType _itype;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"itype", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public winprint.Printer.InterfaceType itype
    {
      get { return _itype; }
      set { _itype = value; }
    }
    private readonly global::System.Collections.Generic.List<int> _category = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(5, Name=@"category", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> category
    {
      get { return _category; }
    }
  
    private string _desc;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"desc", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string desc
    {
      get { return _desc; }
      set { _desc = value; }
    }
    [global::ProtoBuf.ProtoContract(Name=@"Status")]
    public enum Status
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ONLINE", Value=0)]
      ONLINE = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"OFFLINE", Value=1)]
      OFFLINE = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"LESSPAPER", Value=2)]
      LESSPAPER = 2
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"PrinterType")]
    public enum PrinterType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"TYPE58", Value=0)]
      TYPE58 = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"TYPE80", Value=1)]
      TYPE80 = 1
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"InterfaceType")]
    public enum InterfaceType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"USB", Value=0)]
      USB = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"WIFI", Value=1)]
      WIFI = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ETH", Value=2)]
      ETH = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"COM", Value=3)]
      COM = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"DRIVER", Value=4)]
      DRIVER = 4
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Data")]
  public partial class Data : global::ProtoBuf.IExtensible
  {
    public Data() {}
    
    private winprint.Result _result = null;
    [global::ProtoBuf.ProtoMember(1, IsRequired = false, Name=@"result", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public winprint.Result result
    {
      get { return _result; }
      set { _result = value; }
    }
    private readonly global::System.Collections.Generic.List<winprint.HeartBeat> _heartbeat = new global::System.Collections.Generic.List<winprint.HeartBeat>();
    [global::ProtoBuf.ProtoMember(2, Name=@"heartbeat", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<winprint.HeartBeat> heartbeat
    {
      get { return _heartbeat; }
    }
  
    private readonly global::System.Collections.Generic.List<winprint.Receipt> _receipts = new global::System.Collections.Generic.List<winprint.Receipt>();
    [global::ProtoBuf.ProtoMember(3, Name=@"receipts", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<winprint.Receipt> receipts
    {
      get { return _receipts; }
    }
  
    private readonly global::System.Collections.Generic.List<winprint.Printer> _printers = new global::System.Collections.Generic.List<winprint.Printer>();
    [global::ProtoBuf.ProtoMember(4, Name=@"printers", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<winprint.Printer> printers
    {
      get { return _printers; }
    }
  
    private readonly global::System.Collections.Generic.List<winprint.NotifyOrders> _notifyorders = new global::System.Collections.Generic.List<winprint.NotifyOrders>();
    [global::ProtoBuf.ProtoMember(5, Name=@"notifyorders", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<winprint.NotifyOrders> notifyorders
    {
      get { return _notifyorders; }
    }
  
    private readonly global::System.Collections.Generic.List<winprint.ImageReceipt> _img_receipts = new global::System.Collections.Generic.List<winprint.ImageReceipt>();
    [global::ProtoBuf.ProtoMember(6, Name=@"img_receipts", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<winprint.ImageReceipt> img_receipts
    {
      get { return _img_receipts; }
    }
  
    private winprint.ServerConfig _servercfg = null;
    [global::ProtoBuf.ProtoMember(7, IsRequired = false, Name=@"servercfg", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public winprint.ServerConfig servercfg
    {
      get { return _servercfg; }
      set { _servercfg = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"Message")]
  public partial class Message : global::ProtoBuf.IExtensible
  {
    public Message() {}
    
    private winprint.Header _header;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"header", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public winprint.Header header
    {
      get { return _header; }
      set { _header = value; }
    }
    private winprint.Data _data = null;
    [global::ProtoBuf.ProtoMember(2, IsRequired = false, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    [global::System.ComponentModel.DefaultValue(null)]
    public winprint.Data data
    {
      get { return _data; }
      set { _data = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
}