# CLAUDE.md — RGRTRASPORTE

## Stack

- **Backend**: C# (.NET), estrutura em camadas — `Application/`, `Dominio/`,
  `Infra.CrossCutting/`, `Infra.Data/`, `Infra.Ioc/`, `Service/`,
  `Hangfire.Worker/` (jobs em background), `Teste/`. Solução: `RGRTRASPORTE.sln`.
- **CI**: Jenkins (`Jenkinsfile` na raiz) — não GitHub Actions.
- **PR aberto** (#5, não mergeado): consolida `RGRFRONT` (Angular) e `RGRAPP`
  (Flutter) para dentro deste repositório como `frontend/` e `mobile/`, via
  `git subtree` (histórico preservado). Depois de mergeado, este vira um
  monorepo com os três. Ver o PR para o motivo e as pendências (Jenkins
  precisa ser reconfigurado para observar o monorepo antes dos repos de
  origem serem arquivados).

## Ship24 — pipeline autônomo (Alibaba Coding Plan)

Este repo tem um board de roadmap na org: **[Roadmap — RGR Transporte](https://github.com/orgs/RGR-Softwarer/projects/1)**.
Mover um item para `Horizonte = Agora` (com descrição preenchida) promoveria
automaticamente para uma issue `ship24:queued` neste repo — mas essa ponte
**ainda não está configurada** para este repositório (não está em
`roadmap-intake.json`, no ambiente do Gustavo).

**Status aqui (2026-08-07)**: nem intake nem execução autônoma estão ativos.
Só o board de roadmap existe até agora. Antes de ligar qualquer automação:
resolver o PR #5 pendente e a questão do Jenkins (CI externo que não
aparece para ferramentas que só olham GitHub Actions).
