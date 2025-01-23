DO $$
BEGIN
    DROP SCHEMA IF EXISTS "public" CASCADE;
    CREATE SCHEMA "public";
    GRANT ALL ON SCHEMA "public" TO "postgres";
    GRANT ALL ON SCHEMA "public" TO "public";
END $$;

CREATE TABLE IF NOT EXISTS "job_config" (
    "id" BIGSERIAL PRIMARY KEY,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "created_by_id" BIGINT NOT NULL,
    "updated_by_id" BIGINT,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "row_version" BIGINT NOT NULL DEFAULT txid_current(),
    "name" VARCHAR(200) NOT NULL,
    "assembly" VARCHAR(500) NOT NULL
);

CREATE TABLE IF NOT EXISTS "job" (
    "id" BIGSERIAL PRIMARY KEY,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "created_by_id" BIGINT NOT NULL,
    "updated_by_id" BIGINT,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "row_version" BIGINT NOT NULL DEFAULT txid_current(),
    "effective_date_time" TIMESTAMPTZ NOT NULL,
    "description" VARCHAR(500),
    "type" INTEGER NOT NULL,
    "cron_expression" VARCHAR(50)
);

CREATE TABLE IF NOT EXISTS "recurring_detail" (
    "id" BIGSERIAL PRIMARY KEY,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "created_by_id" BIGINT NOT NULL,
    "updated_by_id" BIGINT,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "row_version" BIGINT NOT NULL DEFAULT txid_current(),
    "job_id" BIGINT NOT NULL REFERENCES "job"("id"),
    "recurring_type" INTEGER NOT NULL,
    "second" INTEGER NOT NULL,
    "minute" INTEGER NOT NULL,
    "hours" INTEGER NOT NULL,
    "day_of_week" INTEGER NOT NULL,
    "day" INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS "job_step" (
    "id" BIGSERIAL PRIMARY KEY,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "created_by_id" BIGINT NOT NULL,
    "updated_by_id" BIGINT,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "row_version" BIGINT NOT NULL DEFAULT txid_current(),
    "job_id" BIGINT NOT NULL REFERENCES "job"("id"),
    "job_config_id" BIGINT NOT NULL REFERENCES "job_config"("id"),
    "parameter" TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "job_instance" (
    "id" BIGSERIAL PRIMARY KEY,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "created_by_id" BIGINT NOT NULL,
    "updated_by_id" BIGINT,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "row_version" BIGINT NOT NULL DEFAULT txid_current(),
    "job_id" BIGINT NOT NULL REFERENCES "job"("id"),
    "status" INTEGER NOT NULL
);

CREATE TABLE IF NOT EXISTS "job_step_instance" (
    "id" BIGSERIAL PRIMARY KEY,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "created_by_id" BIGINT NOT NULL,
    "updated_by_id" BIGINT,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "row_version" BIGINT NOT NULL DEFAULT txid_current(),
    "job_instance_id" BIGINT NOT NULL REFERENCES "job_instance"("id"),
    "job_step_id" BIGINT NOT NULL REFERENCES "job_step"("id"),
    "status" INTEGER NOT NULL,
    "start_time" TIMESTAMPTZ NOT NULL,
    "end_time" TIMESTAMPTZ
);

CREATE TABLE IF NOT EXISTS "job_step_instance_log" (
    "id" BIGSERIAL PRIMARY KEY,
    "created_time" TIMESTAMPTZ NOT NULL DEFAULT NOW(),
    "updated_time" TIMESTAMPTZ,
    "created_by_id" BIGINT NOT NULL,
    "updated_by_id" BIGINT,
    "active" BOOLEAN NOT NULL DEFAULT TRUE,
    "row_version" BIGINT NOT NULL DEFAULT txid_current(),
    "job_step_instance_id" BIGINT NOT NULL REFERENCES "job_step_instance"("id"),
    "log" TEXT NOT NULL
);

INSERT INTO "job_config" (
    "name", "assembly", "active", "created_time", "created_by_id"
) VALUES (
    'Test', 'Job.ProjectLayer.Test,Job.ProjectLayer', TRUE, CURRENT_TIMESTAMP, 1
);