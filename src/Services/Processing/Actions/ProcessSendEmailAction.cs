using System.Net.Mail;
using Data;
using FluentEmail.Core;
using Serilog;

namespace Services.Processing.Actions;

public class ProcessSendEmailAction(IFluentEmail fluentEmail) : IProcessAction
{
    public async Task ExecuteAsync(RuleAction action, ApplicationContext context, CancellationToken cancellationToken)
    {
        var log = Log.ForContext<ProcessSendEmailAction>();
        
        var senderEmail = action.SendEmailSenderEmail ?? "no-reply@tanbyiot.app";

        if (string.IsNullOrWhiteSpace(action.SendEmailToEmail))
        {
            log.Error("Cannot send email, no recipients given ({RuleActionId})", action.Id);
            return;
        }

        var recipients = action.SendEmailToEmail.Split(';', ',')
            .Select(x => x.Trim());

        foreach (var recipient in recipients)
        {
            try
            {
                log.Information(
                    "Sending email to {To} with subject line {Subject} ({RuleActionId})",
                    recipient,
                    action.SendEmailSubject,
                    action.Id);

                await fluentEmail
                    .SetFrom(senderEmail, action.SendEmailSenderName)
                    .To(recipient)
                    .Subject(action.SendEmailSubject)
                    .Body(action.SendEmailBody)
                    .SendAsync(cancellationToken);
                
                log.Information(
                    "Sent email to {To} with subject line {Subject} ({RuleActionId})",
                    recipient,
                    action.SendEmailSubject,
                    action.Id);
            }
            catch (Exception ex)
            {
                log.Error(ex, "Error sending email to {To} with subject line {Subject} ({RuleActionId})",
                    recipient,
                    action.SendEmailSubject,
                    action.Id);
            }
        }
    }
}