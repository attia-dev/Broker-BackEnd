import { Component, Injector, OnDestroy, OnInit } from '@angular/core';
import { NbMediaBreakpointsService, NbMenuService, NbSidebarService, NbThemeService } from '@nebular/theme';
import { AppAuthService } from '@shared/auth/app-auth.service';
import { UserData } from '../../../@core/data/users';
import { LayoutService } from '../../../@core/utils';
import { map, takeUntil } from 'rxjs/operators';
import { Subject } from 'rxjs';
import { AppComponentBase } from '@shared/app-component-base';
import { ChangeUserLanguageDto, UserDto, UserServiceProxy } from '@shared/service-proxies/service-proxies';
import { filter as _filter } from 'lodash-es';

@Component({
    selector: 'ngx-header',
    styleUrls: ['./header.component.scss'],
    templateUrl: './header.component.html',
})
export class HeaderComponent extends AppComponentBase implements OnInit, OnDestroy {

    private destroy$: Subject<void> = new Subject<void>();
    userPictureOnly: boolean = false;
    user: any;
    languages: abp.localization.ILanguageInfo[];
    currentLanguage: abp.localization.ILanguageInfo;

    themes = [
        {
            value: 'default',
            name: 'Light',
        },
        {
            value: 'dark',
            name: 'Dark',
        },
        {
            value: 'cosmic',
            name: 'Cosmic',
        },
        {
            value: 'corporate',
            name: 'Corporate',
        },
    ];

    currentTheme = 'default';

    userMenu = [/*{ title: 'Profile', link: '/profile' }, { title: 'Change Password', link: '/account/changePassword' },*/{ title: this.l("Logout") , link: '/account/logout' }];

    langMenu = [];

    constructor(injector: Injector, private sidebarService: NbSidebarService,
        private menuService: NbMenuService,
        private themeService: NbThemeService,
        private userService: UserData,
        private layoutService: LayoutService, private _userService: UserServiceProxy,
        private breakpointService: NbMediaBreakpointsService, private _authService: AppAuthService) {
        super(injector);
        this.menuService.onItemClick()
            .subscribe((event) => {
                this.onMenuItemClick(event.item.title);
            });
    }

    ngOnInit() {
        this.currentTheme = this.themeService.currentTheme;
        this.user = this.appSession.user;

        const { xl } = this.breakpointService.getBreakpointsMap();
        this.themeService.onMediaQueryChange()
            .pipe(
                map(([, currentBreakpoint]) => currentBreakpoint.width < xl),
                takeUntil(this.destroy$),
            )
            .subscribe((isLessThanXl: boolean) => this.userPictureOnly = isLessThanXl);

        this.themeService.onThemeChange()
            .pipe(
                map(({ name }) => name),
                takeUntil(this.destroy$),
            )
            .subscribe(themeName => this.currentTheme = themeName);


        this.languages = _filter(this.localization.languages, (l) => !l.isDisabled);
        this.currentLanguage = this.localization.currentLanguage;
        this.languages.forEach((x) => {
            if (x.name != this.currentLanguage.name) {
                this.langMenu.push({ title: x.displayName });
            }
        });
        //console.log("this.currentLanguage", this.currentLanguage);
       // console.log("this.langMenu", this.langMenu);
    }

    ngOnDestroy() {
        this.destroy$.next();
        this.destroy$.complete();
    }

    changeTheme(themeName: string) {
        this.themeService.changeTheme(themeName);
    }

    toggleSidebar(): boolean {
        this.sidebarService.toggle(true, 'menu-sidebar');
        this.layoutService.changeLayoutSize();

        return false;
    }

    navigateHome() {
        this.menuService.navigateHome();
        return false;
    }


    onMenuItemClick(title) {
        this.languages.forEach((x) => {
            if (x.displayName == title && title != this.currentLanguage.displayName) {
                this.changeLanguage(x.name);
            }
        });
    }

    changeLanguage(languageName: string): void {
        const input = new ChangeUserLanguageDto();
        input.languageName = languageName;

        this._userService.changeLanguage(input).subscribe(() => {
            abp.utils.setCookieValue(
                'Abp.Localization.CultureName',
                languageName,
                new Date(new Date().getTime() + 5 * 365 * 86400000), // 5 year
                abp.appPath
            );

            window.location.reload();
        });
    }

    logout(): void {
        this._authService.logout();
    }

}
