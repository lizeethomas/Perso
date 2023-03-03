import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";
import { LoginService } from "../services/login.service";


@Injectable({
    providedIn: 'root'
})

export class LoginGuard implements CanActivate {


    constructor(private loginService: LoginService, private router: Router){

    }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (this.loginService.token$.pipe().subscribe()){
            return true;
        } else {
            this.router.navigateByUrl('/login');
            return false;
        }
    }

}