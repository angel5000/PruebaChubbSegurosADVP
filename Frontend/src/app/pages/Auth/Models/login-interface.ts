export interface Login{
    credential: string,
    password: string
    
    }
    export interface Logout{
        credenciales: string

        }
        export interface UserData{
            identificacion: string
            userName: string
            mail: string
    nombres: string
    apellidos: string
    fechaNacimiento: Date
    fechaIngreso: Date
}
         
export interface Autenticacion {
    usuarioId: number; // probablemente se ignora en el backend al crear
    correo: string;
    contrasena: string;
  }
  
  export interface RegistrarUsuario {
    nombres: string;
    apellidos: string;
    cedula: string;
    direccion: string;
    genero: string;
    autenticacion: Autenticacion[];
  }