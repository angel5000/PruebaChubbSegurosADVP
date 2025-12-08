import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { catchError, map, Observable, of, throwError } from 'rxjs';
import { BaseResponse } from '../../../../shared/models/BaseApiResponse';
import { environment as env } from '../../../../../enviroments/environment';
import { endpoint as end } from '../../../../shared/apis/endpoints';
@Injectable({
  providedIn: 'root'
})
export class CobrazansService {
  constructor(private http: HttpClient, private toastr: ToastrService) { }

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


  Obtenerdatos(): Observable<BaseResponse> {
    return this.http.get<any>(env.apiseguros + end.CONSULTACOBRANZA, { headers: this.getAuthHeaders() }).pipe(

      map((res) => res),
      catchError((error) => {
        this.toastr.error('Error al obtener los datos');
        return throwError(() => error);
      })
    );
  }

  CancelarSeguro(id: number, usuariomod:string): Observable<BaseResponse> {
    const token = localStorage.getItem("token")?.replace(/"/g, '');
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
    const requestURL = `${env.apiseguros}${end.CANCELARSEGURO}${id}?usuario=${usuariomod}`;

    // Eliminamos la propiedad { body: ... }
    return this.http.delete<BaseResponse>(requestURL, { headers }).pipe(
      map(res => res),
      catchError(error => {
        this.showError(error);
        return of(error);
      })
    );

  }




}

