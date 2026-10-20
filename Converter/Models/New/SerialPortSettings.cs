using System.IO.Ports;

namespace Converter.New;

public class SerialPortSettings
{
    public string? PortName { get; set; }

    public int? Baudrate { get; set; }

    public Parity? Parity { get; set; }

    public int? DataBits { get; set; }

    public StopBits? StopBits { get; set; }

    public Handshake? Handshake { get; set; }

    public int? ReadTimeoutMs { get; set; }

    public int? WriteTimeoutMs { get; set; }
}