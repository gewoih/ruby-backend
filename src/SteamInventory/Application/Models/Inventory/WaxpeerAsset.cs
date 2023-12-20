namespace SteamInventory.Application.Models.Inventory
{
    public sealed class WaxpeerAsset
    {
        /// <summary>
        /// ID пользователя Steam, который владеет предметом.
        /// </summary>
        public long SteamUserId { get; set; }

        /// <summary>
        /// Игра и ее ID, к которой относится предмет.
        /// </summary>
	    public SteamGame SteamGame { get; set; }

        /// <summary>
        /// Уникальный идентификатор предмета в рамках конкретного контекста и игры. Не является уникальным в рамках всего Steam.
        /// </summary>
        public long AssetId { get; set; }

        /// <summary>
        /// Название предмета на торговой площадке Steam
        /// </summary>
        public string MarketName { get; set; }

        /// <summary>
        /// Доступен ли предмет для торговли в Steam
        /// </summary>
        public bool Marketable { get; set; }

        /// <summary>
        /// URL на изображение предмета на сервере Steam
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Стоимость предмета на торговой площадке Steam
        /// </summary>
        public double MarketPrice { get; set; }
	}
}
