import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import * as moment from 'moment';

@Component({
    selector: 'ngx-footer',
    styleUrls: ['./footer.component.scss'],
    template: `
    <span class="created-by">
      {{ poweredBy }} <b><a href="https://nahrdev.com/" target="_blank">{{nahr}}</a></b> {{currentYear}}
    </span>
    <div class="socials">
      <a href="#" target="_blank" class="ion ion-social-facebook"></a>
      <a href="#" target="_blank" class="ion ion-social-twitter"></a>
      <a href="#" target="_blank" class="ion ion-social-linkedin"></a>
    </div>
  `,
})
export class FooterComponent extends AppComponentBase implements OnInit {

    currentYear = moment().format("YYYY");
    poweredBy = this.l("Common.PoweredBy");
    nahr = this.l("Common.NahrDev");

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
        
    }

}
