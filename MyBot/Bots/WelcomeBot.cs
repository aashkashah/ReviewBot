using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
namespace MyBot.Bots
{
    public class WelcomeBot<T> : DialogBot<T> where T : Dialog
    {
        public WelcomeBot(ConversationState conversationState, UserState userState, T dialog, ILogger<DialogBot<T>> logger)
            : base(conversationState, userState, dialog, logger)
        {
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext,
            CancellationToken cancellationToken)
        {
            var reply = MessageFactory.Text($"Welcome to Movie review. Type anything to get started.");
            await turnContext.SendActivityAsync(reply, cancellationToken);
        }
    }
}
