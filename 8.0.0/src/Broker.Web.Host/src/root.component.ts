//import { Component } from '@angular/core';
//
//@Component({
//    selector: 'app-root',
//    template: `<router-outlet></router-outlet>`
//})
//export class RootComponent {
//
//}
import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { AppComponentBase } from '@shared/app-component-base';
import { Subscription } from 'rxjs';
import { environment } from './environments/environment';

@Component({
    selector: 'app-root',
    template: `<router-outlet></router-outlet>`
})
export class RootComponent extends AppComponentBase implements OnInit, OnDestroy {

    currentLanguage: abp.localization.ILanguageInfo;
    private unsubscribe: Subscription[] = [];

    constructor(injector: Injector, private router: Router) {
        super(injector);
        this.currentLanguage = this.localization.currentLanguage;
    }

    ngOnInit() {
        //   console.log(this.currentLanguage.name);
        //   alert(this.currentLanguage.name);
        /*if (environment.production) {
            if (location.protocol === 'http:') {
                window.location.href = location.href.replace('http', 'https');
            }
        }*/
        setTimeout(() => {
            debugger;
            if (this.currentLanguage.name.startsWith("ar")) {
                //$("html").attr("dir", "rtl");
                document.documentElement.lang = "ar";
                document.documentElement.dir = "rtl";
                $("nb-sidebar").removeClass("left");
                $("nb-sidebar").addClass("right");
            }
            else{  //if (this.currentLanguage.name.startsWith("en"))
                //$("html").attr("dir", "ltr");
                document.documentElement.lang = "en";
                document.documentElement.dir = "ltr";
                $("nb-sidebar").removeClass("right");
                $("nb-sidebar").addClass("left");
            }
        }, 1000);
        const routerSubscription = this.router.events.subscribe((event) => {
            if (event instanceof NavigationEnd) {
                debugger;
                if (this.currentLanguage.name.startsWith("ar")) {
                    //$("html").attr("dir", "rtl");
                    document.documentElement.lang = "ar";
                    document.documentElement.dir = "rtl";
                    $("nb-sidebar").removeClass("left");
                    $("nb-sidebar").addClass("right");
                }
                else {
                    //$("html").attr("dir", "ltr");
                    document.documentElement.lang = "en";
                    document.documentElement.dir = "ltr";
                    $("nb-sidebar").removeClass("right");
                    $("nb-sidebar").addClass("left");
                }
            }
        });

        /*this.unsubscribe.push(routerSubscription);*/
    }

    ngOnDestroy() {
        //this.unsubscribe.forEach((sb) => sb.unsubscribe());
    }

}