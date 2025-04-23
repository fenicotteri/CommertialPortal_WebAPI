namespace CommertialPortal_WebAPI;

public sealed record EmailData(
    string EmailToId,
    string EmailToName,
    string EmailSubject,
    string EmailBody);
