export interface Asegurados{
    idasegurados:number
    cedula: string;
    nmbrcompleto: string;
    telefono: number;
    edad: number;
    fechacontrataseguro?:string
  }
  export interface AseguradosRequest {
    cedula: string;
    nmbrcompleto: string;
    telefono: number;
    edad: number;
    usrCreacion?: string,
    usrActualizacion?: string,
    fechaActualizacion?: Date
    usuarioIP?: string,
    estado?: number
    estadoDt?:string
  
  }
  export interface Aseguramiento {
    nmbrcompleto?: string;
    nmbrseguro?: string;
    cedula: string;
    codseguro: string;
  }
  