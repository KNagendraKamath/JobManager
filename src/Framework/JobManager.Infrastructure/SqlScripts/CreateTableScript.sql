DO $$
BEGIN
    DROP SCHEMA IF EXISTS JOB CASCADE;
    CREATE SCHEMA JOB;
    GRANT ALL ON SCHEMA job TO "postgres";
    GRANT ALL ON SCHEMA job TO "public";
END $$;

CREATE TABLE IF NOT EXISTS job."job_config" (
    "id" BIGSERIAL PRIMARY KEY,
    "name" VARCHAR(200) NOT NULL UNIQUE,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "row_version" BIGINT NOT NULL DEFAULT txid_current()
);

CREATE TABLE IF NOT EXISTS job."job" (
    "id" BIGSERIAL PRIMARY KEY,
    "effective_date_time" TIMESTAMPTZ NOT NULL,
    "description" VARCHAR(500),
    "type" Varchar(50) NOT NULL,
    "cron_expression" VARCHAR(50),
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "row_version" BIGINT NOT NULL DEFAULT txid_current()
);

CREATE TABLE IF NOT EXISTS job."recurring_detail" (
    "id" BIGSERIAL PRIMARY KEY,
    "job_id" BIGINT NOT NULL REFERENCES job."job"("id"),
    "recurring_type" Varchar(50) NOT NULL,
    "second" INTEGER NOT NULL,
    "minute" INTEGER NOT NULL,
    "hour" INTEGER NOT NULL,
    "day_of_week" Varchar(50) NOT NULL,
    "day" INTEGER NOT NULL,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "row_version" BIGINT NOT NULL DEFAULT txid_current()
);

CREATE TABLE IF NOT EXISTS job."job_step" (
    "id" BIGSERIAL PRIMARY KEY,
    "job_id" BIGINT NOT NULL REFERENCES job."job"("id"),
    "job_config_id" BIGINT NOT NULL REFERENCES job."job_config"("id"),
    "parameter" TEXT NOT NULL,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "row_version" BIGINT NOT NULL DEFAULT txid_current()
);

CREATE TABLE IF NOT EXISTS job."job_instance" (
    "id" BIGSERIAL PRIMARY KEY,
    "job_id" BIGINT NOT NULL REFERENCES job."job"("id"),
    "status" Varchar(50) NOT NULL,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "row_version" BIGINT NOT NULL DEFAULT txid_current()
);

CREATE TABLE IF NOT EXISTS job."job_step_instance" (
    "id" BIGSERIAL PRIMARY KEY,
    "job_instance_id" BIGINT NOT NULL REFERENCES job."job_instance"("id"),
    "job_step_id" BIGINT NOT NULL REFERENCES job."job_step"("id"),
    "status" Varchar(50) NOT NULL,
    "start_time" TIMESTAMPTZ,
    "end_time" TIMESTAMPTZ,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "row_version" BIGINT NOT NULL DEFAULT txid_current()
);

CREATE TABLE IF NOT EXISTS job."job_step_instance_log" (
    "id" BIGSERIAL PRIMARY KEY,
    "job_step_instance_id" BIGINT NOT NULL REFERENCES job."job_step_instance"("id"),
    "log" TEXT NOT NULL,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "row_version" BIGINT NOT NULL DEFAULT txid_current()
);