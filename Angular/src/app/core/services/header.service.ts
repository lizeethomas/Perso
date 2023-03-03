import { Injectable } from "@angular/core";
import { BehaviorSubject } from "rxjs";

@Injectable()

export class HeaderService {

    status = new BehaviorSubject<boolean>(false);
    logged = new BehaviorSubject<string>("");
    role = new BehaviorSubject<string>("");

    setStatus() : void {
        let test:boolean = false;
        if (localStorage.getItem("token")) {
            test = true;
            this.role.next(<string>localStorage.getItem("role"))
            this.logged.next(<string>localStorage.getItem("username")); 
        }
        this.status.next(test);
    }

}