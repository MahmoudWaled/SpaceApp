using Microsoft.AspNetCore.SignalR;
using Space.Application.DTOs.Messages;
using Space.Application.Interfaces.Services;

namespace Space.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService messageService;

        public ChatHub(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        public async Task SendMessage(string senderId, string receiverId, string message)
        {
            try
            {
                var messageDto = new CreateMessageDto
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = message
                };
                await messageService.SendMessageAsync(messageDto);
                await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
            }
            catch (InvalidDataException ex)
            {
                throw new HubException($"Failed to send message: {ex.Message}");
            }
        }

        public override Task OnConnectedAsync()
        {
            var userId = Context.UserIdentifier ?? Context.ConnectionId;
            return base.OnConnectedAsync();
        }

        public async Task MarkMessageAsSeen(string messageId)
        {
            var message = await messageService.MarkMessageAsSeenAsync(messageId);
            if (message != null)
            {
                await Clients.User(message.SenderId).SendAsync("MessageSeen", messageId);
            }
        }

        public async Task SendTyping(string senderId, string receiverId)
        {
            if (string.IsNullOrEmpty(senderId) || string.IsNullOrEmpty(receiverId))
            {
                throw new ArgumentException("SenderId and ReceiverId cannot be null or empty.");
            }
            await Clients.User(receiverId).SendAsync("UserTyping", senderId);
        }
    }
}