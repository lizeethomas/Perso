import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HeaderService } from '../../services/header.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  constructor(private headerService: HeaderService, private router: Router) {

  }

  logged:string = "";
  status: boolean = false;

  ngOnInit() {
    this.verifValidity();
    this.headerService.status.subscribe(newStatus => {
      this.status = newStatus;
    })
    this.headerService.logged.subscribe(newUser => {
      this.logged = newUser;
    })
    this.headerService.setStatus();
  }

  logout() {
    localStorage.clear();
    this.headerService.setStatus();
    this.router.navigateByUrl(`login`);
  }

  verifValidity() : void {
    let expire:string | null = localStorage.getItem("expire");
    if (expire !== null) {
      var date = Date.parse(expire);
      var now = Date.now();
      if (now > date) {
        this.logout();
      } else {
        console.log(date);
        console.log(now);
      }
    }
  }
  
}
