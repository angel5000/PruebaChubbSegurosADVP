
export interface SegurosResponse {
  idseguro: number
  nmbrseguro: string;
  codseguro: string;
  sumasegurada: number;
  prima: number;
  rangoEdad: string;
}

export interface SegurosResponseID {
  idseguro: string
  nmbrseguro: string;
  codseguro: string;
  sumasegurada: number;
  prima: number;
  edadmin: number;
  edadmax: number;
}
export interface SegurosRequest {
  nmbrseguro: string;
  codseguro: string;
  sumasegurada: number;
  prima: number;
  edadmin: number;
  edadmax: number;
}