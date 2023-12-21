export class LoginDto {

    constructor(
      public email: string,
      public senha: string,
      public lembrar: boolean,
      public mensagemErro: string
    ) {  }
  
  }