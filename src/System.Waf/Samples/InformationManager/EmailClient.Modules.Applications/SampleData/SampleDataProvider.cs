﻿using System.CodeDom.Compiler;
using Waf.InformationManager.EmailClient.Modules.Domain.AccountSettings;
using Waf.InformationManager.EmailClient.Modules.Domain.Emails;

namespace Waf.InformationManager.EmailClient.Modules.Applications.SampleData;

[GeneratedCode("ToSuppressCodeAnalysis", "1.0.0.0")]
public static class SampleDataProvider
{
    public static EmailAccount CreateEmailAccount()
    {
        var emailAccount = new EmailAccount() { Name = "Harry Thompson", Email = "harry@example.com" };
        emailAccount.EmailAccountSettings = new ExchangeSettings() { ServerPath = "exchange.example.com", UserName = "harry" };
        return emailAccount;
    }

    public static IReadOnlyList<Email> CreateInboxEmails() => [
        CreateEmail("Aliquam cras class maecenas", new DateTime(2003, 11, 5, 8, 20, 14), "someone@example.com", new[] { "harry@example.com", "user-2@fabrikam.com", "someone@adventure-works.com" }, new[] { "someone-4@adventure-works.com" }, "Vestibulum aenean mauris maecenas pellentesque nullam aliquam cras adipiscing class nam suspendisse praesent parturient integer condimentum consectetuer vestibulum curabitur curae pellentesque vestibulum scelerisque phasellus duis sed nunc donec amet accumsan ante sollicitudin quisque dis arcu consequat etiam fusce convallis dignissim adipiscing elementum pellentesque facilisis fermentum fringilla diam aptent consectetuer vivamus lorem aliquam morbi eget habitasse pellentesque aliquet hendrerit blandit sollicitudin elit nulla pellentesque proin est enim commodo conubia bibendum ullamcorper himenaeos parturient hac leo dictumst augue suspendisse erat mus cubilia condimentum scelerisque auctor imperdiet congue vestibulum vestibulum cursus dapibus eros nibh eleifend ullamcorper adipiscing egestas facilisi parturient vestibulum malesuada"),
        CreateEmail("Duis nunc", new DateTime(2006, 12, 29, 7, 6, 5), "user@adventure-works.com", new[] { "harry@example.com", "admin@adventure-works.com" }, null, "Euismod dolor penatibus vestibulum nec suspendisse feugiat dictum fames porttitor gravida nisi felis faucibus consectetuer non lectus condimentum ipsum pellentesque justo libero nisl per iaculis tincidunt sed sollicitudin ligula habitant pellentesque lacinia nunc inceptos interdum lacus lorem sem litora scelerisque ullamcorper luctus lobortis tristique consectetuer sit suspendisse pellentesque condimentum adipiscing odio parturient nascetur scelerisque magnis orci pede ultricies mattis mauris vestibulum pharetra vel venenatis vulputate nam sed"),
        CreateEmail("Proin nam", new DateTime(2004, 9, 19, 19, 13, 20), "user-1@fabrikam.com", new[] { "user-6@fabrikam.com" }, new[] { "harry@example.com" }, "Sapien sollicitudin amet ante pellentesque semper dapibus consectetuer scelerisque adipiscing pellentesque lobortis nascetur sociis arcu egestas pharetra himenaeos sit proin ullamcorper diam taciti parturient vel sollicitudin eget suspendisse nam placerat elit pulvinar euismod sed condimentum enim imperdiet scelerisque pellentesque malesuada sagittis vestibulum ullamcorper erat suspendisse penatibus consectetuer augue senectus dolor condimentum pellentesque vestibulum porttitor sollicitudin"),
        CreateEmail("Auctor congue diam accumsan", new DateTime(2003, 8, 10, 23, 5, 39), "user-7@fabrikam.com", new[] { "harry@example.com" }, null, "Fames sociosqu feugiat dis pellentesque eros tincidunt consectetuer felis suscipit gravida est torquent ultrices pellentesque scelerisque tristique adipiscing iaculis ultricies nibh tellus ipsum sollicitudin tempor pellentesque consectetuer justo ullamcorper parturient nisi vestibulum suspendisse hac venenatis vehicula lacus vestibulum pellentesque lacinia sollicitudin condimentum volutpat maecenas tempus vulputate tortor adipiscing curabitur laoreet nisl"),
        CreateEmail("Aliquet eleifend blandit", new DateTime(2006, 12, 29, 16, 26, 00), "someone@adventure-works.com", new[] { "harry@example.com" }, new[] { "mike@adventure-works.com" }, "Proin facilisi imperdiet augue ante faucibus commodo hac vestibulum habitant pellentesque leo malesuada arcu inceptos interdum diam lobortis nascetur penatibus pharetra conubia condimentum dictum lectus cubilia adipiscing libero eget parturient dolor fames consectetuer elit mus pellentesque enim felis dapibus vestibulum ligula nec placerat porttitor vestibulum scelerisque erat ipsum tincidunt ullamcorper pulvinar sagittis justo lacus lorem suspendisse adipiscing litora sollicitudin parturient vestibulum non condimentum pellentesque luctus tristique eros senectus nibh sociosqu magnis consectetuer per"),
        CreateEmail("Egestas nisi mattis", new DateTime(2010, 1, 19, 7, 13, 4), "user-2@fabrikam.com", new[] { "harry@example.com" }, null, "Scelerisque nisl ultricies suscipit magna torquent venenatis massa metus vulputate sed curabitur pellentesque nunc morbi ullamcorper mauris vestibulum sollicitudin neque odio phasellus mollis sem consequat convallis sit orci ultrices dignissim euismod feugiat pellentesque elementum vel vehicula adipiscing montes gravida netus iaculis volutpat parturient facilisis maecenas lacinia praesent consectetuer nulla accumsan fermentum nostra suspendisse nam ornare porta vestibulum bibendum condimentum fringilla laoreet dictumst vestibulum habitasse purus hendrerit adipiscing risus pede himenaeos sed pellentesque"),
        CreateEmail("Lorem tempor", new DateTime(2006, 2, 2, 17, 44, 29), "user-6@fabrikam.com", new[] { "user-7@fabrikam.com", "harry@example.com" }, null, "Venenatis amet vulputate sed morbi ante curabitur phasellus aliquet nulla pellentesque arcu tempus tortor turpis diam nascetur sollicitudin blandit pellentesque eget consequat convallis proin parturient dignissim vestibulum elit sem elementum commodo augue condimentum facilisis scelerisque sit varius conubia pharetra aenean vestibulum vel ullamcorper fermentum dolor fames consectetuer placerat enim nam"),
        CreateEmail("Felis sed pulvinar ipsum", new DateTime(2003, 8, 23, 23, 38, 50), "user-8@fabrikam.com", new[] { "harry@example.com" }, null, "Cubilia dis sagittis mauris nullam senectus aptent sociosqu suscipit adipiscing parturient fringilla justo habitasse torquent lacus suspendisse est auctor erat lorem eros condimentum hac pellentesque ultrices vestibulum nibh sollicitudin leo magna massa congue cursus nisi vestibulum vehicula pellentesque mus nec consectetuer nisl dapibus non scelerisque hendrerit pellentesque metus per ullamcorper morbi dictum adipiscing suspendisse neque lectus nunc egestas libero condimentum parturient odio volutpat maecenas praesent euismod ligula scelerisque feugiat vestibulum vestibulum ullamcorper sollicitudin netus orci sed adipiscing suspendisse nulla litora accumsan gravida pede quam condimentum scelerisque bibendum dictumst quis ullamcorper himenaeos urna eleifend porta luctus parturient imperdiet suspendisse iaculis magnis"),
        CreateEmail("Nunc sed dis suscipit", new DateTime(2012, 8, 9, 5, 58, 21), "user-2@fabrikam.com", new[] { "harry@example.com" }, new[] { "user-3@fabrikam.com" }, "Scelerisque est odio orci montes pellentesque vulputate nostra euismod parturient ornare hac vestibulum vestibulum proin platea augue adipiscing dolor pede leo torquent quam primis curabitur rutrum ultrices vehicula sollicitudin parturient pellentesque consectetuer pellentesque fames sollicitudin volutpat sapien pellentesque vestibulum phasellus semper maecenas feugiat sociis consequat felis vestibulum ipsum justo mus praesent gravida ullamcorper consectetuer pellentesque taciti suspendisse adipiscing iaculis nec tellus quis tempor convallis parturient tempus non lacinia condimentum dignissim sollicitudin tortor per elementum scelerisque accumsan urna ullamcorper facilisis laoreet bibendum turpis varius suspendisse lacus dictumst cras sed aenean eleifend"),
        CreateEmail("Taciti enim", new DateTime(2005, 9, 5, 16, 34, 45), "someone-2@adventure-works.com", new[] { "harry@example.com" }, null, "Sed condimentum proin sodales vestibulum scelerisque erat vehicula sem augue adipiscing sit parturient vel volutpat maecenas viverra ullamcorper suspendisse nam praesent dolor vestibulum condimentum vestibulum sed pellentesque fames adipiscing eros nibh venenatis felis parturient aliquam dis est hac integer nisi scelerisque accumsan quisque bibendum ullamcorper dictumst ipsum vulputate curabitur vestibulum justo lacus sollicitudin"),
    ];

    public static IReadOnlyList<Email> CreateSentEmails()
    {
        var emails = new List<Email>()
        {
            CreateEmail("Sed dis", new DateTime(2011, 9, 26, 23, 11, 12), "harry@example.com", new[] { "someone-6@adventure-works.com", "someone-7@adventure-works.com" }, null, "Nulla porta cras fringilla duis est scelerisque ullamcorper adipiscing habitasse pellentesque lacinia purus risus lobortis nunc amet hendrerit nullam himenaeos hac consectetuer velit laoreet parturient leo aptent pellentesque ante vitae sollicitudin mus nec non class pellentesque suspendisse curae auctor condimentum donec scelerisque imperdiet nascetur congue ullamcorper pharetra suspendisse consectetuer placerat arcu vestibulum cursus dictum per lectus pellentesque vestibulum condimentum diam malesuada scelerisque penatibus natoque porttitor ullamcorper"),
            CreateEmail("Etiam eget sed", new DateTime(2001, 4, 24, 16, 32, 44), "harry@example.com", new[] { "someone@example.com" }, new[] { "user@adventure-works.com", "user-2@adventure-works.com" }, "Pulvinar fusce suspendisse libero sagittis posuere lorem morbi tincidunt sem sollicitudin potenti pretium sit tristique elit nulla adipiscing ultricies pellentesque condimentum vel enim rhoncus erat eros ligula nibh proin consectetuer senectus sodales litora scelerisque luctus augue dolor magnis ullamcorper suspendisse condimentum nam fames venenatis scelerisque vulputate nisi ullamcorper pellentesque felis viverra nisl parturient ipsum justo sociosqu nunc mattis lacus mauris odio sed sollicitudin vestibulum suscipit torquent dis curabitur vestibulum aliquam suspendisse pellentesque condimentum adipiscing"),
            CreateEmail("Massa sed", new DateTime(2012, 4, 7, 10, 7, 5), "harry@example.com", new[] { "user-2@fabrikam.com" }, null, "Metus elementum duis morbi neque condimentum dapibus egestas facilisis euismod consectetuer nunc fermentum sem amet pellentesque nostra ante feugiat arcu diam sollicitudin sit netus vel gravida fringilla dictumst eget nam eleifend vestibulum scelerisque sed ullamcorper suspendisse habitasse iaculis facilisi condimentum pellentesque nulla porta faucibus ornare consectetuer purus scelerisque pellentesque dis est ullamcorper platea risus suspendisse primis habitant sollicitudin elit condimentum velit pellentesque rutrum"),
            CreateEmail("Magnis urna mattis egestas", new DateTime(2008, 1, 16, 16, 4, 49), "harry@example.com", new[] { "someone-4@adventure-works.com", "someone-5@adventure-works.com" }, null, "Venenatis vulputate curabitur phasellus cras vestibulum ullamcorper duis vestibulum euismod feugiat suspendisse nunc condimentum pharetra scelerisque amet mauris ullamcorper nam ante pellentesque arcu sollicitudin consequat sed convallis adipiscing parturient vestibulum mollis pellentesque diam dis consectetuer vestibulum pellentesque netus nulla montes nostra placerat suspendisse ornare adipiscing parturient eget dignissim platea pulvinar sollicitudin porta primis gravida sagittis elementum senectus facilisis pellentesque est hac leo iaculis consectetuer mus lacinia fermentum elit condimentum scelerisque ullamcorper pellentesque enim sociosqu vestibulum fringilla nec vestibulum purus suspendisse sollicitudin non rutrum erat adipiscing per habitasse parturient suscipit risus eros"),
            CreateEmail("Potenti etiam", new DateTime(2005, 1, 16, 13, 57, 21), "harry@example.com", new[] { "someone@example.com" }, null, "Pretium urna rhoncus fusce condimentum hac sodales lorem ultricies morbi nulla viverra aliquam integer proin augue scelerisque ullamcorper suspendisse praesent leo mus pellentesque condimentum sollicitudin tellus scelerisque quisque ullamcorper nec pellentesque suspendisse vivamus non accumsan venenatis aliquam consectetuer pellentesque aliquet dolor vestibulum cras adipiscing blandit fames condimentum tempor sollicitudin scelerisque felis parturient pellentesque duis tempus"),
        };
        emails.ForEach(x => x.EmailType = EmailType.Sent);
        return emails;
    }

    public static IReadOnlyList<Email> CreateDrafts()
    {
        var emails = new List<Email>()
        {
            CreateEmail("Sociis nunc vivamus sagittis", new DateTime(2006, 7, 1, 3, 40, 49), "harry@example.com", null, null, "Parturient nulla aliquam scelerisque senectus pellentesque porta odio sociosqu suscipit aliquet vestibulum sollicitudin purus blandit ullamcorper vestibulum adipiscing pellentesque")
        };
        emails.ForEach(x => x.EmailType = EmailType.Sent);
        return emails;
    }

    private static Email CreateEmail(string title, DateTime sent, string from, IReadOnlyList<string>? to, IReadOnlyList<string>? cc, string message)
    {
        var email = new Email() { Title = title, Message = message, Sent = sent, From = from };
        if (to != null) email.To = to;
        if (cc != null) email.CC = cc;
        return email;
    }
}
