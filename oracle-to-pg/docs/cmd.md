cd /mnt/c/Repos/github/sql-builder-web/oracle-to-pg

# Convert Oracle VIEW to PostgreSQL (use ora2pg-view.conf)
ora2pg -c ora2pg-view.conf -i ora-sql/nv_account_sost_nal-processed.sql -o pg-sql/nv_account_sost_nal.sql
ora2pg -c ora2pg-view.conf -i ora-sql/nv_account-processed.sql -o pg-sql/nv_account.sql

# Convert Oracle PACKAGE (use ora2pg.conf)
# ora2pg -c ora2pg.conf -i ora-sql/<package>.sql -o pg-sql/<package>.sql