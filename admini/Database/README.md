# PostgreSQL database files

`antique_shop_portable.sql` is the complete database snapshot for the coursework demo. Product and history records are retained; account names are generic, password values require local reset, and contact fields are redacted.

The project connects to `127.0.0.1:5433`, database `antique_shop`, role `antique_app`. Keep the database password in the local Windows environment variable `ANTIQUE_SHOP_DB_PASSWORD` and never commit it.

To restore: create a local PostgreSQL database named `antique_shop`, configure the `antique_app` role with a password of your choice, then import `antique_shop_portable.sql` as a database administrator. After import, set permissions for `antique_app` on the public schema and tables. Set the same password in `ANTIQUE_SHOP_DB_PASSWORD`, then restart the application.

Seed account password hashes were removed from this export. Before signing in, set new local passwords using the application's password hashing method. The `09_individual_user_passwords.sql` file is intentionally a safe placeholder and does not restore the original demo credentials.

`01_create_database.sql` through `08_demo_catalog_and_history.sql` and `10_remove_employee_role.sql`–`11_existing_proposal_photos.sql` describe the schema and optional updates. The portable dump is the easiest way to restore the included sample state.