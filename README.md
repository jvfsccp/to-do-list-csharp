# Lista de Tarefas (C# / Windows Forms / SQLite)

Aplicação desktop "to-do" com dados salvos em banco SQLite.

## Como executar

Requisitos: **.NET 8 SDK** (ou Visual Studio 2022 com a carga de trabalho "Desenvolvimento para desktop com .NET").

```bash
dotnet run --project ListaTarefas
```

No Visual Studio, abra `ListaTarefas/ListaTarefas.csproj` e pressione F5.

O banco é criado automaticamente no primeiro uso, em `%LocalAppData%\ListaTarefas\tarefas.db`
(o log de erros técnicos fica em `erros.log` na mesma pasta). Não é preciso instalar servidor de banco.

## Uso e atalhos

| Ação | Atalho |
|---|---|
| Nova tarefa | `Ctrl+N` |
| Editar | `F2`, `Enter` ou duplo clique |
| Concluir / reabrir | `Ctrl+Enter` ou clique na caixa "Feito" |
| Excluir | `Del` |
| Subir / descer na lista | `Ctrl+↑` / `Ctrl+↓` |

Tarefas atrasadas (data de término anterior a hoje e não concluídas) aparecem em vermelho. Concluídas ficam ocultas
por padrão; marque "Mostrar concluídas" para vê-las.

## Arquitetura

```
ListaTarefas/
  Models/       Tarefa, DirecaoMovimento                  (dados)
  Services/     TarefaService                             (regras de negócio)
  Data/         ITarefaRepository, TarefaRepository,      (SQL e conexão)
                ConexaoFactory, BancoInicializador
  Exceptions/   RegraNegocioException, AcessoDadosException
  Infra/        PastaDeDados, LogErros
  Forms/        MainForm, TarefaForm, ExecutorSeguro      (somente interface)
  Program.cs    raiz de composição (monta as camadas)
```

Dependência: `Forms → Services → Data`. Os formulários não têm SQL nem regras de negócio.

## Onde cada critério de avaliação é atendido

| Critério | Onde |
|---|---|
| Separação de camadas | Estrutura acima; `Program.cs` injeta o repositório no serviço e o serviço nos formulários |
| Parâmetros contra SQL Injection | Todas as consultas em `Data/TarefaRepository.cs` usam `@parametros` |
| Ciclo de vida das conexões | `using` + `Open()` por operação em `TarefaRepository.Consultar` |
| Transações | `TarefaRepository.Mover` (troca de posição = dois `UPDATE` atômicos) |
| Validação de formato | `Forms/TarefaForm.cs`: `ErrorProvider`, `MaskedTextBox` e `DateTime.TryParseExact` |
| Validação de regras de negócio | `Services/TarefaService.Validar` (título obrigatório, tamanhos, término ≥ início) |
| Tratamento de exceções | `SqliteException` capturada no repositório e convertida em `AcessoDadosException` (mensagem amigável, erro técnico no log); `ExecutorSeguro` exibe a mensagem sem pilha |
| Configuração segura | Connection string em `appsettings.json`; nenhuma credencial no código (SQLite não usa senha) |
| Ergonomia | `TabIndex` ordenado, foco inicial no título, teclas de acesso (`&`), `AcceptButton`/`CancelButton`, atalhos acima |
| Qualidade do código | Nomes consistentes em português com prefixos de controle (`txt`, `btn`, `dgv`, `msk`, `lbl`) |
