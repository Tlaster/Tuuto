﻿using Microsoft.Toolkit.Uwp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tuuto.Model;
using Mastodon.Model;
using Mastodon.Api;
using Tuuto.Common.Helpers;
using Tuuto.Common.Controls;
using Microsoft.EntityFrameworkCore;

namespace Tuuto.Common
{
    static class DraftManager
    {
        public static async void Add(DraftModel item)
        {
            using (var db = new DraftDbContext())
            {
                if (!db.Draft.Any(d => d.Id == item.Id))
                {
                    if (item.Medias != null && item.Medias.Any())
                        foreach (var media in item.Medias)
                            await media.SaveToFile();
                    db.Draft.Add(item);
                    //if (item.Medias != null && item.Medias.Any())
                    //    db.MediaData.AddRange(item.Medias);
                    //if (item.ReplyStatus != null)
                    //    db.Reply.Add(item.ReplyStatus);
                    await db.SaveChangesAsync();
                }
            }
            Task.Run(() => HandleDraft(item));
        }

        private static async Task HandleDraft(DraftModel item)
        {
            var message = ResourceHelper.GetString("SendDraft_OK");
            try
            {
                var result = await SendDraft(item);
                HandleSuccessAsync(item);
            }
            catch (Exception e)
            {
                message = ResourceHelper.GetString("SendDraft_Error");
                HandleFailedAsync(item.Id, e);
            }
            finally
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                 {
                     Notification.Show(message);
                 }, Windows.UI.Core.CoreDispatcherPriority.Normal);
            }
        }

        private static async void HandleSuccessAsync(DraftModel item)
        {
            if (item.Medias != null && item.Medias.Any())
                foreach (var media in item.Medias)
                    await media.DeleteFile();
            using (var db = new DraftDbContext())
            {
                db.Draft.Remove(db.Draft.Find(item.Id));
                await db.SaveChangesAsync();
            }
        }

        private static async void HandleFailedAsync(int id, Exception e)
        {
            using (var db = new DraftDbContext())
            {
                var item = db.Draft.Find(id);
                item.ErrorMessage = e.Message;
                db.Draft.Update(item);
                await db.SaveChangesAsync();
            }
        }

        private static async Task<StatusModel> SendDraft(DraftModel item)
        {
            var medias = new List<AttachmentModel>();
            if (item.Medias?.Count > 0)
                foreach (var media in item.Medias)
                {
                    if (!string.IsNullOrEmpty(media.SavedFile))
                        medias.Add(await Media.Uploading(item.Domain, item.AccessToken, await StorageFileHelper.ReadBytesFromLocalFileAsync(media.SavedFile)));
                    else
                        medias.Add(await Media.Uploading(item.Domain, item.AccessToken, media.Data));
                }
            return await Statuses.Posting(item.Domain, item.AccessToken, item.Status, item.ReplyStatus?.InReplyToId ?? 0, item.Sensitive, item.SpoilerText, item.Visibility, medias.Select(m => m.Id).ToArray());
        }
        public static IEnumerable<DraftModel> GetCurrent()
        {
            using (var db = new DraftDbContext())
            {
                return db.Draft.Include(d => d.ReplyStatus).Include(d => d.Medias).Where(d => d.AccountId == Settings.CurrentAccount.Id && Settings.CurrentAccount.Domain == d.Domain).ToList();
            }
        }
        public static async Task DeleteDraft(DraftModel item)
        {
            using (var db = new DraftDbContext())
            {
                db.Draft.Remove(db.Draft.Find(item.Id));
                await db.SaveChangesAsync();
            }
        }
    }
}
