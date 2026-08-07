export class AppContext {
    constructor(
      public username: string,
      public token: string,
      public nome: string,
      public email?: string,
      public avatar?: string
    ) {}    
  }
  