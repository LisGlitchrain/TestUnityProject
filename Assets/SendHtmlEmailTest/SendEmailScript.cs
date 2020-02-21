using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.IO;

public class SendEmailScript : MonoBehaviour
{
    public string fromAddress = "glitchsmtptest@gmail.com";
    public string toAddress = "hlambdafox@gmail.com";
    public string account = "glitchsmtptest";
    public string password = "u7DZ6rhz7ZPfZ24";
    public string emailFileName = "email1.html";
    public bool isBodyHtml;
    public string mailBody;

    // Start is called before the first frame update
    void Start()
    {
        if(mailBody != string.Empty)
        {
            SendSMTPEmail();
        }
        else
        {
            print($"Path: {Application.streamingAssetsPath}");
            print($"Path: {Application.streamingAssetsPath}/{emailFileName}");
            if (File.Exists($"{Application.streamingAssetsPath}/{emailFileName}"))
            {
                mailBody = File.ReadAllText($"{Application.streamingAssetsPath}/{emailFileName}");
                SendSMTPEmail();
            }
            else
            {
                print("<color=red>ERROR!</color> File does not exist!");
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SendSMTPEmail()
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromAddress);
        mail.To.Add(toAddress);
        mail.Subject = "Test Smtp Mail";
        mail.Body = mailBody;// "Testing SMTP mail from GMAIL";
        mail.IsBodyHtml = isBodyHtml;
        mail.Attachments.Add(new Attachment($"{Application.streamingAssetsPath}/{emailFileName}"));
        // you can use others too.
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Port = 587;
        smtpServer.Credentials = new System.Net.NetworkCredential(account, password) as ICredentialsByHost;
        smtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        { return true; };
        smtpServer.Send(mail);
    }

    void AttachPics(MailMessage mail)
    {

    }

    void AttachPic(MailMessage mail,string attachmentPath, string contentId)
    {
        mail.Attachments.Add(new Attachment(attachmentPath){ ContentId = contentId});
    }
    public void SendEmail()
    {
        string email = toAddress;
        string subject = MyEscapeURL("My Subject");
        string body = MyEscapeURL("My Body\r\nFull of non-escaped chars");

        Application.OpenURL("mailto:" + email + "?subject=" + subject + "&body=" + body);
    }
    string MyEscapeURL(string URL)
    {
        return WWW.EscapeURL(URL).Replace("+", "%20");

    }
}
