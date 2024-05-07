import { Component, OnInit } from '@angular/core';
/*import { AuthService } from '.../_services/auth.service';*/
import { AppAuthService } from '@shared/auth/app-auth.service';

@Component({
    selector: 'app-logout',
    templateUrl: './logout.component.html',

})
export class LogoutComponent implements OnInit {
    constructor(private appAuthService: AppAuthService) {
        debugger;
        this.appAuthService.logout();
    }

    ngOnInit(): void { }
}