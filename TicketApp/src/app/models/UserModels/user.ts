export class User {
    id!: number;
    username!: string;
    email!: string;
    roles!: Role[];
    reportsToId!:string;
    reportsToUsername!:string;
  }

  export class Role {
    id!: number;
    name!: string;
  }

  export class TokenResponse
  {
    token!:string;
  }

  export class LogModel 
  {
    email!:string;
    password!:string;
  }