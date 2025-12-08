import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../../pages/Auth/Services/auth.service';

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router, private authService: AuthService) {}

  canActivate(
    route: ActivatedRouteSnapshot
  ): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
 
    const role = this.authService.getUserRole();
   console.log(role);
   

      if (role !== null ) {
        if (route.url[0].path === 'seguros' && role === 100||role === 101) {
          return true; 
        }
        if (route.url[0].path === 'asegurados' && role === 100||role === 101) {
          return true; 
        }
        if (route.url[0].path === 'cobranzas' && role===101||role === 101) {
         
          return true;
        }
        else if (route.url[0].path === 'bienvenido' ) {
         
            return true;
          }
        else{
            this.router.navigate(['/notfound']);
            return false;
            }
     
      }
      else{
     
          this.router.navigate(['/login']);
          return false;
        
        
      }

    
    }
}