namespace Yap.Models.Chat;

/// <summary>What <c>openDm</c> returns: the tail of the conversation, who types in it, and its pinned messages (chat-hub.md, SH-48).</summary>
public sealed record Conversation(Guid ChannelId, IReadOnlyList<MessageDto> Messages, bool HasMore, IReadOnlyList<string> Typists, IReadOnlyList<Pin> Pins);

/// <summary>What <c>older</c> returns (TL-06).</summary>
public sealed record Page(IReadOnlyList<MessageDto> Messages, bool HasMore);

/// <summary>What <c>around</c> returns (SH-49, MR-34, TL-41): the messages around the target, whether older ones exist
/// (HasMore) and whether newer ones exist (HasNewer). Error is the refusal the reader sees (GEN-02); the list is empty then.
/// The error travels in the body because hub.invoke turns every exception into "Something went wrong".</summary>
public sealed record Window(IReadOnlyList<MessageDto> Messages, bool HasMore, bool HasNewer, string? Error = null);

/// <summary>One entry of <c>messageLinks</c> (TL-43): where a message is, who wrote it and when. DmUser is the other
/// person of a direct message, because the reader's route to it is /dm/{user}; it is null for a room. A message the
/// reader may not read has no entry, so an entry never tells more than the reader could read in the conversation.</summary>
public sealed record MessageLinkDto(Guid Id, Guid ChannelId, string? DmUser, string Username, DateTime Timestamp);

/// <summary>One result of <c>search</c> (SR-08). It has the fields that the client's pin entry drawing reads
/// (<see cref="Pin"/>: message id, author, text, time, the media under the text), so one drawing serves both. The
/// media are the message's own files; a result shows no link card.</summary>
public sealed record SearchHit(Guid MessageId, string Username, string Text, DateTime Timestamp, IReadOnlyList<PinMedia> Media);

/// <summary>What <c>search</c> returns: one page of results, newest first. Total is the count of every match and comes
/// with the first page only (SR-11). HasMore says that a call with the last hit as the cursor finds more. Since is the
/// time of the oldest message the reader can see, null when there is none (SR-12). Error is the refusal the reader
/// sees, in the body for the reason <see cref="Window"/> gives.</summary>
public sealed record SearchPage(IReadOnlyList<SearchHit> Hits, int? Total, bool HasMore, DateTime? Since, string? Error = null);

/// <summary>A refusal from a hub method, with the reason the user reads (GEN-02). A hub method returns null on success.</summary>
public sealed record HubError(string Error);

/// <summary>A direct message with at least one message: the partner and when it last moved (SH-29, SH-30).</summary>
public sealed record DmDto(Guid ChannelId, string Partner, DateTime LastMessageAt);
