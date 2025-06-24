# 📋 Plano de Migração do ng-zorro-antd

## 🎯 **Objetivo**
Migrar gradualmente do ng-zorro-antd para componentes customizados, mantendo a funcionalidade e melhorando a performance.

---

## 📅 **Fase 1: Preparação e Infraestrutura (Semana 1)**

### **1.1 Setup de Alternativas**
```bash
# Instalar dependências alternativas
npm install bootstrap @fortawesome/fontawesome-free
npm install @angular/material @angular/cdk
```

### **1.2 Criar Biblioteca de Componentes Base**
```
src/
├── shared/
│   ├── components/
│   │   ├── button/
│   │   ├── card/
│   │   ├── modal/
│   │   ├── table/
│   │   └── form/
│   └── services/
│       ├── notification.service.ts
│       └── modal.service.ts
```

### **1.3 Configurar CSS Framework**
- Implementar Bootstrap ou Tailwind CSS
- Criar variáveis CSS customizadas
- Definir sistema de cores e tipografia

---

## 📅 **Fase 2: Layout e Navegação (Semana 2)**

### **2.1 Migrar Layout Principal (Home)**
**Antes:**
```html
<nz-layout class="main-layout">
  <nz-sider>...</nz-sider>
  <nz-layout>
    <nz-header>...</nz-header>
    <nz-content>...</nz-content>
    <nz-footer>...</nz-footer>
  </nz-layout>
</nz-layout>
```

**Depois:**
```html
<div class="main-layout">
  <aside class="sidebar">...</aside>
  <main class="content">
    <header class="header">...</header>
    <section class="content-area">...</section>
    <footer class="footer">...</footer>
  </main>
</div>
```

### **2.2 Migrar Menu Lateral**
- Substituir `nz-menu` por `<nav>` customizado
- Implementar dropdown customizado para submenus
- Manter funcionalidade de colapso

### **2.3 Migrar Breadcrumb**
- Criar componente breadcrumb customizado
- Substituir `nz-breadcrumb`

---

## 📅 **Fase 3: Componentes Básicos (Semana 3)**

### **3.1 Botões**
**Antes:**
```html
<button nz-button nzType="primary">Salvar</button>
```

**Depois:**
```html
<button class="btn btn-primary">Salvar</button>
```

### **3.2 Cards**
**Antes:**
```html
<nz-card nzTitle="Título">Conteúdo</nz-card>
```

**Depois:**
```html
<div class="card">
  <div class="card-header">Título</div>
  <div class="card-body">Conteúdo</div>
</div>
```

### **3.3 Ícones**
- Substituir `nz-icon` por Font Awesome
- Criar diretiva para ícones
- Manter compatibilidade com nomes de ícones existentes

---

## 📅 **Fase 4: Formulários (Semana 4)**

### **4.1 Campos de Input**
**Antes:**
```html
<nz-form-item>
  <nz-form-label>Nome</nz-form-label>
  <nz-form-control>
    <input nz-input formControlName="nome" />
  </nz-form-control>
</nz-form-item>
```

**Depois:**
```html
<div class="form-group">
  <label class="form-label">Nome</label>
  <input class="form-control" formControlName="nome" />
</div>
```

### **4.2 Select e Checkbox**
- Criar componentes customizados para select
- Implementar checkbox customizado
- Manter funcionalidade de validação

---

## 📅 **Fase 5: Tabelas e Grid (Semana 5)**

### **5.1 Tabelas Simples**
**Antes:**
```html
<nz-table [nzData]="data">
  <thead>
    <tr>
      <th nzColumnKey="name">Nome</th>
    </tr>
  </thead>
  <tbody>
    <tr *ngFor="let item of data">
      <td>{{ item.name }}</td>
    </tr>
  </tbody>
</nz-table>
```

**Depois:**
```html
<table class="table">
  <thead>
    <tr>
      <th>Nome</th>
    </tr>
  </thead>
  <tbody>
    <tr *ngFor="let item of data">
      <td>{{ item.name }}</td>
    </tr>
  </tbody>
</table>
```

### **5.2 Funcionalidades Avançadas**
- Implementar paginação customizada
- Criar sistema de ordenação
- Adicionar filtros customizados

---

## 📅 **Fase 6: Modais e Notificações (Semana 6)**

### **6.1 Sistema de Modais**
- Criar serviço de modal customizado
- Implementar componente modal reutilizável
- Substituir `NzModalService`

### **6.2 Sistema de Notificações**
- Criar serviço de notificação customizado
- Implementar toast notifications
- Substituir `NzMessageService` e `NzNotificationService`

---

## 📅 **Fase 7: Componentes Complexos (Semana 7)**

### **7.1 Dashboard**
- Migrar cards de estatísticas
- Substituir gráficos e tabelas
- Manter funcionalidade de configuração

### **7.2 Grid Component**
- Migrar tabelas complexas
- Implementar funcionalidades de filtro
- Manter sistema de ações

---

## 📅 **Fase 8: Limpeza e Otimização (Semana 8)**

### **8.1 Remover Dependências**
```bash
npm uninstall ng-zorro-antd @ant-design/icons-angular
```

### **8.2 Limpar Imports**
- Remover imports não utilizados
- Limpar módulos Angular
- Otimizar bundle size

### **8.3 Testes e Ajustes**
- Testar todas as funcionalidades
- Ajustar estilos e responsividade
- Otimizar performance

---

## 🛠️ **Estrutura de Arquivos Proposta**

```
src/
├── shared/
│   ├── components/
│   │   ├── button/
│   │   │   ├── button.component.ts
│   │   │   ├── button.component.html
│   │   │   └── button.component.scss
│   │   ├── card/
│   │   ├── modal/
│   │   ├── table/
│   │   └── form/
│   ├── services/
│   │   ├── notification.service.ts
│   │   └── modal.service.ts
│   └── shared.module.ts
├── styles/
│   ├── _variables.scss
│   ├── _buttons.scss
│   ├── _cards.scss
│   ├── _tables.scss
│   └── _forms.scss
└── assets/
    └── icons/
```

---

## 📊 **Métricas de Sucesso**

### **Performance**
- Redução de 50% no bundle size
- Melhoria de 30% no tempo de carregamento
- Redução de dependências externas

### **Manutenibilidade**
- Código mais limpo e organizado
- Componentes reutilizáveis
- Melhor controle sobre estilos

### **Funcionalidade**
- Manter 100% das funcionalidades existentes
- Melhorar responsividade
- Adicionar acessibilidade

---

## ⚠️ **Riscos e Mitigações**

### **Riscos**
- Tempo de desenvolvimento maior que o estimado
- Perda de funcionalidades durante migração
- Problemas de compatibilidade

### **Mitigações**
- Migração gradual e incremental
- Testes contínuos em cada fase
- Manter backup do código original
- Documentar todas as mudanças

---

## 🚀 **Status de Execução**

- [x] **Fase 1.1**: Setup de Alternativas - EM ANDAMENTO
- [ ] **Fase 1.2**: Criar Biblioteca de Componentes Base
- [ ] **Fase 1.3**: Configurar CSS Framework
- [ ] **Fase 2**: Layout e Navegação
- [ ] **Fase 3**: Componentes Básicos
- [ ] **Fase 4**: Formulários
- [ ] **Fase 5**: Tabelas e Grid
- [ ] **Fase 6**: Modais e Notificações
- [ ] **Fase 7**: Componentes Complexos
- [ ] **Fase 8**: Limpeza e Otimização

---

**Data de Início**: $(date)
**Responsável**: Equipe de Desenvolvimento
**Versão**: 1.0 