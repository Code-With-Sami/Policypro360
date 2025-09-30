let connection;
let currentConversationId = null;

async function startConnection() {
    connection = new signalR.HubConnectionBuilder()
        .withUrl('/hubs/chat')
        .withAutomaticReconnect()
        .build();

    connection.on('ReceiveMessage', (payload) => {
        if (payload.conversationId === currentConversationId) {
            appendMessage(payload, false);
            connection.invoke('MarkMessagesRead', currentConversationId).catch(e => console.error(e));
        } else {
            incrementUnread(payload.conversationId);
        }
    });

    connection.on('Typing', (conversationId, isTyping) => {
        if (conversationId === currentConversationId) {
            document.getElementById('typing').innerText = isTyping ? 'Typing...' : '';
        }
    });

    connection.on('MessagesMarkedRead', (conversationId) => {
        if (conversationId === currentConversationId) {
            markAllReadUI();
        }
    });

    await connection.start();
    console.log('SignalR connected');
}

async function loadConversations() {
    const res = await fetch('/api/chat/conversations');
    const convs = await res.json();
    renderConversations(convs);
}

function renderConversations(convs) {
    const el = document.getElementById('chatList');
    el.innerHTML = '';
    convs.forEach(c => {
        const div = document.createElement('div');
        div.className = 'conv-item';
        div.id = 'conv-' + c.id;
        div.innerHTML = `
<div class="conv-title">${c.companyId ? 'Company: ' + c.companyId : ''} ${c.userId ? 'User: ' + c.userId : ''}</div>
<div class="conv-last">${c.lastMessage ? c.lastMessage.text : ''}</div>
<div class="conv-unread" data-id="${c.id}">${c.unreadCount || ''}</div>
`;
        div.addEventListener('click', () => openConversation(c.id));
        el.appendChild(div);
    });
}

async function openConversation(convId) {
    if (currentConversationId) {
        try { await connection.invoke('LeaveConversation', currentConversationId); } catch { }
    }
    currentConversationId = convId;
    await connection.invoke('JoinConversation', convId);


    const res = await fetch(`/api/chat/messages/${convId}`);
    const messages = await res.json();
    renderMessages(messages);
    await connection.invoke('MarkMessagesRead', convId);
}

function renderMessages(messages) {
    const el = document.getElementById('messages');
    el.innerHTML = '';
    messages.forEach(m => appendMessage(m, true));
    el.scrollTop = el.scrollHeight;
}

function appendMessage(m, skipScrollAdjust) {
    const el = document.getElementById('messages');
    const div = document.createElement('div');
    div.className = 'msg ' + (m.senderType === 'Company' ? 'from-company' : 'from-user');
    div.innerHTML = `<div class="msg-text">${escapeHtml(m.text || m.text)}</div><div class="msg-time">${new Date(m.createdAt).toLocaleString()}</div>`;
    el.appendChild(div);
    if (!skipScrollAdjust) el.scrollTop = el.scrollHeight;
}

function escapeHtml(text) {
    if (!text) return '';
    return text.replace(/[&<>"']/g, function (c) {
        return { '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', '\'': '&#39;' }[c];
    });
}

function incrementUnread(conversationId) {
    const badge = document.querySelector('.conv-unread[data-id="' + conversationId + '"]');
    if (!badge) return;
    const n = parseInt(badge.innerText || '0') || 0;
    badge.innerText = n + 1;
}

function markAllReadUI() {
    const badge = document.querySelector('.conv-unread[data-id="' + currentConversationId + '"]');
    if (badge) badge.innerText = '';
}

document.addEventListener('DOMContentLoaded', async () => {
    await startConnection();
    await loadConversations();


    document.getElementById('sendBtn').addEventListener('click', async () => {
        const text = document.getElementById('messageText').value;
        if (!text || !currentConversationId) return;
        await connection.invoke('SendMessage', currentConversationId, text);
        document.getElementById('messageText').value = '';
    });


    let typingTimeout;
    document.getElementById('messageText').addEventListener('input', () => {
        if (!currentConversationId) return;
        connection.invoke('Typing', currentConversationId, true);
        clearTimeout(typingTimeout);
        typingTimeout = setTimeout(() => connection.invoke('Typing', currentConversationId, false), 1200);
    });
});