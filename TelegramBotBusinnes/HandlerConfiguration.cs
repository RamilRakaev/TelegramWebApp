﻿using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotBusiness.CallbackQueryHandlers;
using TelegramBotBusiness.MessageHandlers;
using TelegramBotService;

namespace TelegramBotBusiness
{
    public static class HandlerConfiguration
    {
        public static void Configuration(this Handlers handlers)
        {
            handlers.TextMessageHandlers = new List<TextMessageHandler>()
            {
                new TextMessageHandler("/inline_mode",
                "send inline keyboard",
                (ITelegramBotClient botClient, Message message) => InlineHandlers.SendInlineKeyboard(botClient, message)),

                new TextMessageHandler("/keyboard",
                "send custom keyboard",
                (ITelegramBotClient botClient, Message message) => KeyboardHandlers.SendReplyKeyboard(botClient, message)),

                new TextMessageHandler("/remove_keyboard",
                "remove custom keyboard",
                (ITelegramBotClient botClient, Message message) => KeyboardHandlers.RemoveKeyboard(botClient, message)),

                new TextMessageHandler("/photo",
                "send a photo",
                (ITelegramBotClient botClient, Message message) => FileHandlers.SendFile(botClient, message)),

                new TextMessageHandler("/request",
                "request location or contact",
                (ITelegramBotClient botClient, Message message) => RequestHandlers.RequestContactAndLocation(botClient, message))
            };

            handlers.CallbackQueryHandlers = new List<CallbackQueryMessageHandler>()
            {
                new CallbackQueryMessageHandler(
                    "Выведенное сообщение",
                    OutputCallbackQueryHandler.BotOnCallbackQueryReceived)
            };
        }
    }
}