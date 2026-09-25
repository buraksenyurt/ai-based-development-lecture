-- Ids are stored as TEXT (Guid.ToString()). Ordered lists keep their order in the "rank" column
-- (rank 0 = first choice), which is how Rule 00 / Rule 01 preference orders are persisted.

CREATE TABLE IF NOT EXISTS participants (
    participant_id TEXT PRIMARY KEY,
    full_name      TEXT    NOT NULL,
    email          TEXT    NOT NULL,
    university     TEXT    NOT NULL,
    department     TEXT    NOT NULL,
    class          INTEGER NOT NULL,
    github_url     TEXT    NULL
);

-- kind: 'language' | 'database'
CREATE TABLE IF NOT EXISTS participant_preferences (
    participant_id TEXT    NOT NULL REFERENCES participants(participant_id) ON DELETE CASCADE,
    kind           TEXT    NOT NULL CHECK (kind IN ('language', 'database')),
    rank           INTEGER NOT NULL,
    name           TEXT    NOT NULL,
    PRIMARY KEY (participant_id, kind, rank)
);

CREATE TABLE IF NOT EXISTS projects (
    project_id TEXT PRIMARY KEY,
    title      TEXT    NOT NULL,
    summary    TEXT    NOT NULL CHECK (length(summary) <= 250),
    team_min   INTEGER NOT NULL CHECK (team_min >= 1),
    team_max   INTEGER NOT NULL CHECK (team_max >= team_min),
    size       TEXT    NOT NULL
);

-- kind: 'language' | 'platform' | 'database' | 'similar' | 'tag'
CREATE TABLE IF NOT EXISTS project_items (
    project_id TEXT    NOT NULL REFERENCES projects(project_id) ON DELETE CASCADE,
    kind       TEXT    NOT NULL CHECK (kind IN ('language', 'platform', 'database', 'similar', 'tag')),
    rank       INTEGER NOT NULL,
    name       TEXT    NOT NULL,
    PRIMARY KEY (project_id, kind, rank)
);

CREATE TABLE IF NOT EXISTS competitions (
    competition_id TEXT PRIMARY KEY,
    title          TEXT NOT NULL,
    session        TEXT NOT NULL,
    settled_at     TEXT NULL
);

CREATE TABLE IF NOT EXISTS competition_projects (
    competition_id TEXT NOT NULL REFERENCES competitions(competition_id) ON DELETE CASCADE,
    project_id     TEXT NOT NULL REFERENCES projects(project_id),
    PRIMARY KEY (competition_id, project_id)
);

CREATE TABLE IF NOT EXISTS competition_participants (
    competition_id TEXT NOT NULL REFERENCES competitions(competition_id) ON DELETE CASCADE,
    participant_id TEXT NOT NULL REFERENCES participants(participant_id),
    PRIMARY KEY (competition_id, participant_id)
);

-- The primary key guarantees Rule 03 at database level: a participant is placed in one project per competition.
CREATE TABLE IF NOT EXISTS settlements (
    competition_id TEXT NOT NULL,
    project_id     TEXT NOT NULL,
    participant_id TEXT NOT NULL,
    PRIMARY KEY (competition_id, participant_id),
    FOREIGN KEY (competition_id, project_id) REFERENCES competition_projects(competition_id, project_id) ON DELETE CASCADE,
    FOREIGN KEY (competition_id, participant_id) REFERENCES competition_participants(competition_id, participant_id) ON DELETE CASCADE
);
