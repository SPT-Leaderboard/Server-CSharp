﻿using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Utils;
using SPTLeaderboard.Models;
using SPTLeaderboard.Utils;

namespace SPTLeaderboard.Callbacks;

[Injectable]
public class InboxCallbacks(
    SessionInboxChecks inboxChecks,
    HttpResponseUtil httpResponseUtil,
    InboxUtils inboxUtils,
    ILogger<InboxCallbacks> logger)
{
    public ValueTask<object> HandleInboxNotChecked(MongoId sessionId, string? output)
    {
        logger.LogDebug("[SPTLeaderboard] {SessionId} not checked", sessionId);
        if (!inboxChecks.TrySetSessionInboxState(sessionId, false))
        {
            logger.LogDebug("added {SessionId} to inbox checks", sessionId);
            inboxChecks.AddSessionInboxState(sessionId, false);
        }

        return new ValueTask<object>(httpResponseUtil.NoBody(output));
    }

    public ValueTask<object> HandleInboxChecked(MongoId sessionId, string? output)
    {
        logger.LogDebug("{SessionId} is checked", sessionId);
        Task.Run(() => inboxUtils.CheckInbox(sessionId));
        if (!inboxChecks.TrySetSessionInboxState(sessionId, true))
        {
            logger.LogDebug("added {SessionId} to inbox checks", sessionId);
            inboxChecks.AddSessionInboxState(sessionId, true);
        }
        return new ValueTask<object>(httpResponseUtil.NoBody(output));
    }
}