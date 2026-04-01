-- =============================================================
-- PersianHub — Seed Test Images
-- اجرا کن توی SSMS یا Azure Data Studio
-- مرحله ۱: فایل‌های تصویر رو توی wwwroot/uploads/ کپی کن
-- مرحله ۲: این اسکریپت رو اجرا کن
-- =============================================================

-- =============================================================
-- ۱. پروفایل کاربران  →  AppUsers.ProfileImageUrl
-- =============================================================
-- فایل مورد نظر رو با همین نام توی wwwroot/uploads/ بذار
UPDATE AppUsers
SET ProfileImageUrl = 'profile-admin.jpg'
WHERE Email = 'admin@persianhub.se';

UPDATE AppUsers
SET ProfileImageUrl = 'profile-samira.jpg'
WHERE Email = 'samira@example.com';   -- ایمیل واقعی رو جایگزین کن

-- برای دیدن همه کاربرها:
-- SELECT Id, FirstName, LastName, Email, ProfileImageUrl FROM AppUsers;

-- =============================================================
-- ۲. لوگوی بیزینس  →  Businesses.LogoUrl
-- =============================================================
-- برای دیدن همه بیزینس‌ها:
-- SELECT Id, Name, LogoUrl FROM Businesses;

UPDATE Businesses SET LogoUrl = 'logo-restaurant.jpg' WHERE Id = 1;
UPDATE Businesses SET LogoUrl = 'logo-salon.jpg'      WHERE Id = 2;
-- Id های واقعی رو از کوئری بالا بگیر و جایگزین کن

-- =============================================================
-- ۳. گالری تصاویر بیزینس  →  BusinessImages
-- =============================================================
-- ستون‌ها: BusinessId, ImageUrl, AltText, DisplayOrder, IsCover, CreatedAtUtc

INSERT INTO BusinessImages (BusinessId, ImageUrl, AltText, DisplayOrder, IsCover, CreatedAtUtc)
VALUES
  (1, 'gallery-1.jpg', 'Interior view',  0, 1, GETUTCDATE()),  -- IsCover = 1 (cover اول)
  (1, 'gallery-2.jpg', 'Menu items',     1, 0, GETUTCDATE()),
  (1, 'gallery-3.jpg', 'Outside view',   2, 0, GETUTCDATE()),
  (2, 'gallery-4.jpg', 'Salon interior', 0, 1, GETUTCDATE());

-- =============================================================
-- ۴. تصویر رویداد  →  Events.CoverImageUrl
-- =============================================================
-- SELECT Id, Title, CoverImageUrl FROM Events;
UPDATE Events SET CoverImageUrl = 'event-nowruz.jpg' WHERE Id = 1;

-- =============================================================
-- ۵. تصویر آفر  →  DailyOffers.CoverImageUrl
-- =============================================================
-- SELECT Id, Title, CoverImageUrl FROM DailyOffers;
UPDATE DailyOffers SET CoverImageUrl = 'offer-summer.jpg' WHERE Id = 1;

-- =============================================================
-- بررسی نهایی — بعد از اجرا این‌ها رو چک کن
-- =============================================================
SELECT 'AppUsers'      AS [Table], Id, ProfileImageUrl AS ImageColumn FROM AppUsers      WHERE ProfileImageUrl IS NOT NULL
UNION ALL
SELECT 'Businesses'    AS [Table], Id, LogoUrl          AS ImageColumn FROM Businesses    WHERE LogoUrl IS NOT NULL
UNION ALL
SELECT 'BusinessImages'AS [Table], Id, ImageUrl         AS ImageColumn FROM BusinessImages
UNION ALL
SELECT 'Events'        AS [Table], Id, CoverImageUrl    AS ImageColumn FROM Events        WHERE CoverImageUrl IS NOT NULL
UNION ALL
SELECT 'DailyOffers'   AS [Table], Id, CoverImageUrl    AS ImageColumn FROM DailyOffers   WHERE CoverImageUrl IS NOT NULL;
