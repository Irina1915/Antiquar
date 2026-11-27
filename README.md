# Antiquar — Antique Shop Desktop Application

Windows Forms educational application for an antique shop. It provides a searchable product catalog, customer proposals, administrator review, a persistent cart, a simulated checkout, and purchase history.

## Repository contents

- `Antique_shop_forms/` — C# solution, application, library, tests, images, and PostgreSQL scripts.
- `admini/Database/` — current portable PostgreSQL dump and setup scripts.
- `Отчет Байкова.docx` — project defense report.
- `Презентация Байкова.pptx` — presentation for the defense.

## Technology

- C# and .NET 8 Windows Forms (`WinFormsApp2`)
- `AntiqueShopLibrary` for models, validation, and business services
- Npgsql and PostgreSQL
- MSTest unit tests in `TestAvtoriz`

## Database and local setup

The provided portable dump preserves the catalog and history while anonymizing account names and removing personal contact details. Passwords are intentionally reset to `RESET_REQUIRED`; set your own local credentials before testing sign-in. Do not commit passwords, password hashes, or contact details.

The current project configuration expects PostgreSQL at `127.0.0.1:5433`, database `antique_shop`, user `antique_app`. The database password is read from the Windows user environment variable `ANTIQUE_SHOP_DB_PASSWORD`, not from source code. Configure PostgreSQL locally, restore `admini/Database/antique_shop_portable.sql`, set a new database password, then restart Visual Studio.

Open `Antique_shop_forms/Antique_shop_forms.sln` in Visual Studio. The startup application is `WinFormsApp2`. Restore NuGet dependencies before building.

## Demo payment notice

Checkout is a demonstration only. It does not contact a payment provider and must not be used with real card details.

## License

No license is specified yet. Add one before redistributing the code outside the intended coursework context.