-- Esquema do banco SQLite. A aplicação executa este mesmo script sozinha no primeiro uso
-- (ListaTarefas/Data/BancoInicializador.cs); o arquivo existe para documentação.
CREATE TABLE IF NOT EXISTS tarefas (
    id             INTEGER PRIMARY KEY AUTOINCREMENT,
    titulo         TEXT    NOT NULL,
    descricao      TEXT    NULL,
    data_inicio    TEXT    NULL,            -- yyyy-MM-dd
    data_fim       TEXT    NULL,            -- yyyy-MM-dd
    concluida      INTEGER NOT NULL DEFAULT 0 CHECK (concluida IN (0, 1)),
    data_conclusao TEXT    NULL,            -- yyyy-MM-dd HH:mm:ss
    ordem          INTEGER NOT NULL         -- menor = mais importante (topo da lista)
);
