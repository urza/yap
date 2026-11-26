# Yap messenger (the next direction of branch `dmonly`)

This document records a design discussion that started on 2026-11-25. It holds no plan, and no code changed yet.
The branch still works as [dmonly.md](dmonly.md) describes: every person talks to the admin and to nobody else.

In the new direction, dmonly becomes a messenger. Users talk to each other in direct messages. The admin is one
user with admin rights. Groups come later.

The discussion happens on a whiteboard: https://claude.ai/artifact/V6oAyCpyVmVsJj43GCBs5W. The link opens only for
the owner of the board. This document is the readable record of that board. The last update was on 2026-11-26.

Each line below has one of four states:

- **Decided.** The owner of the project decided this.
- **Proposed.** An idea or a proposal with no decision yet.
- **Open.** A question with no answer yet.
- **Today.** How the code works today.

## The core offer

- **Decided.** The messenger is Yap for private conversations. It works in the style of Signal, and it has the
  design, the UI and the features of Yap.
- **Decided.** The Yap experience is the offer. It includes GIFs from Klipy, cached video embeds, good media and
  typing indicators.

## First screen

- **Decided.** Every user gets a list of conversations. The model is the DM section of the Discord phone app. Each
  entry shows the photo, the name and the last line of the conversation.
- **Decided.** This list is the only list of people. A separate friends page does not exist. Search and "add
  contact" are above the list, and Settings is at the bottom.
- **Decided.** Pinned conversations stay at the top of the list, as in Signal.
- **Decided.** A pinned conversation uses the word "pin" and the pin icon, the same as a pinned message. Signal does
  the same.
- **Decided.** The desktop layout takes Discord on the desktop and the web as its model: the list in a left column,
  and the open conversation beside it.
- **Proposed.** Main's frame with its sidebar already has this shape, so the desktop screen can reuse it. The phone
  screen is the new work.
- **Today.** Main already has the data for the list (`ChatService.DmConversations`, made for its sidebar).
- **Today.** The server does not send the last message text of a conversation.

## Finding people

- **Decided.** Users do not see each other. No public list of accounts exists.
- **Decided.** A user adds a person with the exact username. This opens a new conversation.
- **Decided.** A new contact can write one message. The other person then accepts, blocks or deletes.
- **Decided.** That first message is text only. The sender sees no profile and no presence before the accept.
- **Decided.** A new user starts with two contacts: the person who invited them, and the bot.
- **Decided.** A user can give a contact a nickname. No other person sees it. Signal and Discord have the same
  feature.
- **Decided.** The nickname replaces the name of the contact everywhere for this user. That includes the list of
  conversations, the messages and the notifications, and later the groups.
- **Decided.** The nickname is the primary name of the contact. The original username shows in the profile of the
  contact as a secondary line in smaller text.
- **Decided.** The search finds a contact by the nickname and by the username.
- **Proposed.** A user sees only the people who share a conversation with them. This one rule covers direct messages
  and groups.
- **Proposed.** The username search has a rate limit, because an answer shows if a username exists.
- **Proposed.** The server stores the nickname with the account of the user, so each device of the user shows it.
  The server also needs the nickname for the text of a notification.
- **Open.** A request from a stranger needs a place. Signal shows it in the same list, and the conversation opens
  with the buttons Accept, Block and Delete.
- **Today.** dmonly already sends each person a small directory (the admin, the bot, themself).

## Sign-in

- **Decided.** The sign-in must change. It stays without a password, and it becomes more secure than in Yap.
- **Decided.** The passkey comes first. A Nostr key can follow later as a second sign-in method, so an account has
  a list of sign-in methods.
- **Decided.** Smart login is removed.
- **Decided.** The admin creates an invite. The user opens it, picks a username and sets up a passkey.
- **Decided.** The invite stops working after the first passkey exists (or later, the first Nostr key).
- **Decided.** A second device signs in with a synced passkey, or with a QR code that the phone confirms.
- **Decided.** For a lost passkey, the admin makes a new invite for the account.
- **Open.** Can a user also invite, or only the admin?
- **Open.** Is Nostr only a sign-in method, or also an identity that a person carries to other servers?
- **Today.** Smart login signs a device in with the username alone, on a network that the account used before.
- **Today.** Each user has one link. It is the login link, and an unused link is the invite.
- **Today.** Passkeys are built on the branch `worktree-passkeys`. That branch is not merged.

## Conversation theme

- **Proposed.** A user can set a theme for one conversation (the background and the colours), as in Signal. The
  different look shows quickly which conversation is open.
- **Decided.** The theme of a conversation shows only for the user who set it.
- **Decided.** A user can offer their theme of a conversation to the other person. The offer is a special message
  with the button "Use this look", and it stays in the history.
- **Today.** One theme applies to the whole app.

## App lock

- **Proposed.** An optional PIN code locks some conversations.
- **Proposed.** An optional PIN code locks the whole app after the app was closed for some time.
- **Proposed.** A duress PIN code is a possible addition. Its effect is not defined yet.
- **Open.** The names are not decided. "App lock", "Chat lock", "Passcode lock" and "Screen lock" are all possible,
  and "panic passcode" for the duress code. The UI text must keep this code apart from a pinned message and a pinned
  conversation.

## Encryption

- **Decided.** The experience comes before the highest security. Chats are not end-to-end encrypted, and the UI
  says so.
- **Decided.** There is one type of chat. A split into "secret chats" and normal chats is rejected.
- **Proposed.** Encryption for all chats is a topic for a later brainstorm. It must not take away a part of the
  experience.

## The bot

- **Decided.** The bot stays. Each user has the bot as a contact with its own conversation.
- **Today.** On dmonly, the bot writes inside the person's conversation with the admin.

## Groups (later)

- **Decided.** Rooms do not come back. A group is a direct message for more than two people, modelled on Signal.
- **Decided.** A group is never public.
- **Proposed.** A direct message stays as it is on main (two people in two columns). A group becomes a new
  conversation type with a member list.
- **Proposed.** We decide the shape of the member list before step 1, also when the groups are built later. Groups
  need a migration.
- **Proposed.** The members of a group see each other, also when they are not contacts.
- **Today.** The code has no member list. A room on main is open to every account, and a direct message stores two
  people.

## Server, step 1

- **Today.** On main, any two users can already talk in a direct message.
- **Today.** On dmonly, a pair is valid only with the admin on one side (one rule in `ChatService.GetOrCreateDm`).
- **Today.** dmonly limits the directory and the presence to the admin.
- **Proposed.** Step 1 removes these limits. The server change is small, and the first screen is the larger work.

## Branch and main

- **Decided.** dmonly gets its own repository later. For now the work stays on this branch.
- **Proposed.** Main gets small changes with a clear benefit, so that dmonly adds files beside main's files and
  rewrites fewer of them. One example is a neutral test helper in place of `SendToLobby`. A second example is a
  registration pattern for the lists that both sides edit (the routes, the store reset).
- **Proposed.** The messenger design comes first. The design shows where the branch differs from main.
- **Today.** The last sync from main (2026-11-25) had conflicts in 12 of 27 picks.
