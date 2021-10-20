using Google.Apis.Calendar.v3.Data;
using System;
using System.Threading.Tasks;

namespace GoogleCalendarService
{
    public interface IGoogleCalendar
    {
        /// <summary>
        /// Вернуть мероприятия
        /// </summary>
        /// <param name="timeMin">Нижнаяя граница даты окончания события</param>
        /// <param name="timeMax">Верняя граница даты начала события</param>
        /// <param name="maxResults">Максимальное количество возвращаемых событий</param>
        /// <param name="showDeleted">Вернуть удалённые события</param>
        /// <param name="singleEvents">Вернуть повторяющиеся события</param>
        /// <param name="showHiddenInvitations">Показать скрытые события</param>
        /// <param name="q">Произвольный текстовый поиск событий по заданной строке</param>
        /// <param name="sortByModifiedDate">Сортировать по дате обновления</param>
        /// <returns></returns>
        public Task<Event[]> GetEvents(
            DateTime? timeMin = null,
            DateTime? timeMax = null,
            int maxResults = 100,
            bool showDeleted = false,
            bool singleEvents = true,
            bool showHiddenInvitations = false,
            string q = null,
            bool sortByModifiedDate = false);

        /// <summary>
        /// Вернуть список событий в определённом промежутке времени за сегодняшний день
        /// </summary>
        /// <param name="startHours">Часы начала интервала</param>
        /// <param name="startMinutes">Минуты начала интервала</param>
        /// <param name="endHours">Часы конца интервала</param>
        /// <param name="endMinutes">Минуты конца интервала</param>
        /// <returns>string</returns>
        public Task<string> ShowDayEventsInTimeInterval(int startHours, int startMinutes, int endHours, int endMinutes);

        public Task<string> ShowUpCommingEvents(Event[] events = null);
    }
}
