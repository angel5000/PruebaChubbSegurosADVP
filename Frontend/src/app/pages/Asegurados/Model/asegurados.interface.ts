export interface Asegurados{
    idasegurados:number
    cedula: string;
    nmbrcompleto: string;
    telefono: number;
    edad: number;
  }
  export interface AseguradosRequest {
    cedula: string;
    nmbrcompleto: string;
    telefono: number;
    edad: number;
  }
  export interface Aseguramiento {
    nmbrcompleto?: string;
    nmbrseguro?: string;
    cedula: string;
    codseguro: string;
  }
  