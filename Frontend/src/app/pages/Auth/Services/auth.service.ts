import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { BaseResponse } from '../../../shared/models/BaseApiResponse';
import { Login, RegistrarUsuario, UserData } from '../Models/login-interface';
import { environment as env } from '../../../../enviroments/environment';
import { endpoint as end } from '../../../shared/apis/endpoints';
import { catchError, map } from 'rxjs/operators';
import { of } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient, private toastr: ToastrService) {
  }

  private formDataUser: FormData | null = null;
  showSuccess(mensaje: string) {
    this.toastr.success(mensaje, 'Éxito');
  }
  showError(mensaje: string) {
    this.toastr.error(mensaje, 'Error');
  }

  private getAuthHeaders(): HttpHeaders {
    const token = localStorage.getItem("token")?.replace(/"/g, '');
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }
  login(req: Login): Observable<BaseResponse> {
    const requestURL = `${env.apiseguros}${end.AUTH}`
    return this.http.post<BaseResponse>(requestURL, req).pipe(
      map((resp: BaseResponse) => {
        console.log(resp);

        if (resp.isSucces) {
          this.showSuccess(resp.message);
          if (resp.data != null) {
            const correo = resp.data.correo;
            const usuario = resp.data.nombreUsuario;
            const rol = resp.data.idRol;
            const token= resp.data.token;
            localStorage.setItem('rol', JSON.stringify(rol)); 
            localStorage.setItem('correo', JSON.stringify(correo));
            localStorage.setItem('usuario', JSON.stringify(usuario));
            localStorage.setItem('token', JSON.stringify(token));
          }

        } else {
          this.showError(resp.message);
        }
        return resp;
      }), catchError((error) => {
        this.showError(error);
        return of(error);  
      }));
  }
  ObtenerPermisos(id:number): Observable<BaseResponse> {
    return this.http.get<any>(`${env.apiseguros}${end.CONSULTAPERMISOS}${id}`, { headers: this.getAuthHeaders() }).pipe(
       map((res) => res), catchError((error) => {
         this.toastr.error('Error al obtener los datos'); 
         return throwError(() => error); }) );
        }
 

    getUserRole(): number | null {
      return JSON.parse(localStorage.getItem('rol') || 'null');
      
    }
  
    getNombre(): number | null {
      return JSON.parse(localStorage.getItem('usuario') || 'null');
    }
  
    
    


}
