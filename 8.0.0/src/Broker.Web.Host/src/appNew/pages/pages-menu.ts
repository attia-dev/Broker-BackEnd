import { NbMenuItem } from '@nebular/theme';

export const MENU_ITEMS: NbMenuItem[] = [
    {
        title: 'Pages.Dashboard.Title',
        icon: 'home-outline',
        fragment: 'Read.Permission.Dashboards', 
        link: '/admin/pages/dashboard',
        home: true,
    }, 
   /* {
        title: 'Pages.advertisements.Title',
        icon: 'home-outline',
        //fragment: 'Read.Permission.advertisements', 
        link: '/admin/pages/advertisements',
        
    }, */
    
    {
        title: 'Pages.Lookups.Title',
        icon: 'layers',
        fragment: 'Read.Permission.Lookups',
        children: [
            {
                title: 'Pages.Lookups.Countries.Title',
                link: '/admin/pages/lookups/countries',
                fragment: 'Read.Permission.Countries'
            },
            {
                title: 'Pages.Lookups.Governorates.Title',
                link: '/admin/pages/lookups/governorates',
                 fragment: 'Read.Permission.Governorates'
            },
            {
                title: 'Pages.Lookups.Cities.Title',
                link: '/admin/pages/lookups/cities',
                fragment: 'Read.Permission.Cities'
            },
            {
                title: 'Pages.Lookups.Packages.Title',
                link: '/admin/pages/lookups/packages',
                fragment: 'Read.Permission.Packages'
            },
            {
                title: 'Pages.Lookups.Facilities.Title',
                link: '/admin/pages/lookups/facilities',
                fragment: 'Read.Permission.Facilities'
            },
            {
                title: 'Pages.Lookups.MinTimeToBookForChalets.Title',
                link: '/admin/pages/lookups/minTimeToBookForChalets',
               // fragment: 'Read.Permission.Facilities'
            },
         //  {
         //      title: 'Pages.Lookups.Documents.Title',
         //      link: '/admin/pages/lookups/documents',
         //      fragment: 'Read.Permission.Documents'
         //  },
         //  {
         //      title: 'Pages.Lookups.Decorations.Title',
         //      link: '/admin/pages/lookups/decorations',
         //      fragment: 'Read.Permission.Decorations'
         //  },
            {
                title: 'Pages.Lookups.Durations.Title',
                link: '/admin/pages/lookups/durations',
                fragment: 'Read.Permission.Durations'
            },
            {
                title: 'Pages.Lookups.DiscountCodes.Title',
                link: '/admin/pages/lookups/discountCodes',
                fragment: 'Read.Permission.DiscountCodes'
            },
          //  {
          //      title: 'Pages.Lookups.Properties.Title',
          //      link: '/admin/pages/lookups/properties',
          //      fragment: 'Read.Permission.Properties'
          //      
          //  },
            {
                title: 'Pages.Lookups.FeatureAds.Title',
                link: '/admin/pages/lookups/featureAds',
                fragment: 'Read.Permission.FeatureAds'
            },
            {
                title: 'Pages.Lookups.Points.Title',
                link: '/admin/pages/lookups/points',
                fragment: 'Read.Permission.Points'
            },
            {
                title: 'Pages.Lookups.ContactUs.Title',
                link: '/admin/pages/lookups/ContactUs',
                fragment: 'Read.Permission.ContactUs'
                
            },
            {
                title: 'Pages.Lookups.SocialContacts.Title',
                link: '/admin/pages/lookups/SocialContacts',
                //fragment: 'Read.Permission.ContactUs'
                
            },
            
        ]
    },

    {
        title: 'Pages.Users.Title',
        icon: 'layers',
        fragment: 'Read.Permission.Users',
        children: [

            {
                title: 'Pages.BrokerPersons.Title',
                //icon: 'calendar-outline',
                link: '/admin/pages/customers/brokerPersons',
                fragment: 'Read.Permission.Brokers'
            },
            {
                title: 'Pages.Owners.Title',
                //icon: 'calendar-outline',
                link: '/admin/pages/customers/owners',
                fragment: 'Read.Permission.Owners'
            },
            {
                title: 'Pages.Seekers.Title',
                //icon: 'calendar-outline',
                link: '/admin/pages/customers/seekers',
                fragment: 'Read.Permission.Seekers'
            },
            {
                title: 'Pages.Companies.Title',
                //icon: 'calendar-outline',
                link: '/admin/pages/customers/companies',
                fragment: 'Read.Permission.Companies'
            },
        ]
    },
    //{
    //    title: 'Pages.Payments.Title',
    //    icon: 'layers',
    //    //fragment: 'Read.Permission.Lookups',
    //    children: [

    //        {
    //            title: 'Pages.Wallets.Title',
    //            icon: 'calendar-outline',
    //            link: '/admin/pages/payments/wallets',
    //            /*fragment: 'Read.Permission.BrokerPersons'*/
    //        },]
    //},


    {
        title: 'Pages.Advertisements.Title',
        icon: 'calendar-outline',
        link: '/admin/pages/advertisements/tabbedAds',
        fragment: 'Read.Permission.Advertisements'
        
    },
    {
        title: 'Pages.Projects.Title',
        icon: 'calendar-outline',
        link: '/admin/pages/projects/projects',
        //fragment: 'Read.Permission.Projects'
        
    },

    {
        title: 'Pages.ManagePages.Title',
        fragment: 'Read.Permission.ManagePages',
        icon: 'layers',
        children: [
            {
                title: 'Pages.Lookups.About.Title',
                link: '/admin/pages/lookups/about',
                fragment: 'Read.Permission.Abouts'
            },
            //{
            //    title: 'Pages.Lookups.Terms.Title',
            //    link: '/admin/pages/lookups/terms',
            //    fragment: 'Read.Permission.Terms'
            //},
            //{
            //    title: 'Pages.Lookups.Privacy.Title',
            //    link: '/admin/pages/lookups/privacy',
            //    fragment: 'Read.Permission.Privacy'
            //},
            {
                title: 'Pages.Lookups.Contact.Title',
                link: '/admin/pages/lookups/contact',
                fragment: 'Read.Permission.Contacts'
            },

        ],
    },
    {
        title: 'Pages.Ratings.Title',
        icon: 'calendar-outline',
        link: '/admin/pages/ratings/ratings',
        //fragment: 'Read.Permission.Advertisements'
        
    },
    {
        title: 'Pages.Users.UsersManagement',
        icon: 'person-outline',
        link: '/admin/pages/users/users',
        fragment: 'Read.Permission.Admins'

    },
    
  /*  {
        title: 'Pages.Lookups.Title',
        icon: 'layers',
        fragment: 'Read.Permission.Lookups',
        children: [
            //{
            //    title: 'Pages.Lookups.Governorates.Title',
            //    link: '/admin/pages/lookups/governorates',
            //     fragment: 'Read.Permission.Governorates'
            //},
            {
                title: 'Pages.Lookups.cities.Title',
                link: '/admin/pages/lookups/cities',
                fragment: 'Read.Permission.Cities'
            }, {
                title: 'Pages.Lookups.companies.Title',
                link: '/admin/pages/lookups/companies',
                fragment: 'Read.Permission.Companies'
            },
            {
                title: 'Pages.Lookups.lines.Title',
                link: '/admin/pages/lookups/lines',
                fragment: 'Read.Permission.Lines'
            },
            {
                title: 'Pages.Lookups.territories.Title',
                link: '/admin/pages/lookups/territories',
                fragment: 'Read.Permission.Territories'
            },
            {
                title: 'Pages.Lookups.titles.Title',
                link: '/admin/pages/lookups/titles',
                fragment: 'Read.Permission.Titles'
            },
            {
                title: 'Pages.Lookups.bricks.Title',
                link: '/admin/pages/lookups/bricks',
                 fragment: 'Read.Permission.Bricks'
            },
            {
                title: 'Pages.Lookups.distances.Title',
                link: '/admin/pages/lookups/distances',
                fragment: 'Read.Permission.Distances'
            },

        ],
    },

    {
        title: 'Pages.Requests.Title',
        icon: 'calendar-outline',
        link: '/admin/pages/requests/requests',
        fragment: 'Read.Permission.Requests'
    },
    {
        title: 'Pages.Employees.Title',
        icon: 'people-outline',
        link: '/admin/pages/employees/employees',
        fragment: 'Read.Permission.Employees'
    },
   
    */
   

    
    /*
    {
        title: 'Ecommerce',
        icon: 'shopping-cart-outline',
        link: '/admin/pages/ecommerce',
    },
    {
        title: 'FEATURES',
        group: true,
    },
    {
        title: 'Layout',
        icon: 'layout-outline',
        children: [
            {
                title: 'Stepper',
                link: '/admin/pages/layout/stepper',
            },
            {
                title: 'List',
                link: '/admin/pages/layout/list',
            },
            {
                title: 'Infinite List',
                link: '/admin/pages/layout/infinite-list',
            },
            {
                title: 'Accordion',
                link: '/admin/pages/layout/accordion',
            },
            {
                title: 'Tabs',
                pathMatch: 'prefix',
                link: '/admin/pages/layout/tabs',
            },
        ],
    },
    {
        title: 'Forms',
        icon: 'edit-2-outline',
        children: [
            {
                title: 'Form Inputs',
                link: '/admin/pages/forms/inputs',
            },
            {
                title: 'Form Layouts',
                link: '/admin/pages/forms/layouts',
            },
            {
                title: 'Buttons',
                link: '/admin/pages/forms/buttons',
            },
            {
                title: 'Datepicker',
                link: '/admin/pages/forms/datepicker',
            },
        ],
    },
    {
        title: 'UI Features',
        icon: 'keypad-outline',
        link: '/admin/pages/ui-features',
        children: [
            {
                title: 'Grid',
                link: '/admin/pages/ui-features/grid',
            },
            {
                title: 'Icons',
                link: '/admin/pages/ui-features/icons',
            },
            {
                title: 'Typography',
                link: '/admin/pages/ui-features/typography',
            },
            {
                title: 'Animated Searches',
                link: '/admin/pages/ui-features/search-fields',
            },
        ],
    },
    {
        title: 'Modal & Overlays',
        icon: 'browser-outline',
        children: [
            {
                title: 'Dialog',
                link: '/admin/pages/modal-overlays/dialog',
            },
            {
                title: 'Window',
                link: '/admin/pages/modal-overlays/window',
            },
            {
                title: 'Popover',
                link: '/admin/pages/modal-overlays/popover',
            },
            {
                title: 'Toastr',
                link: '/admin/pages/modal-overlays/toastr',
            },
            {
                title: 'Tooltip',
                link: '/admin/pages/modal-overlays/tooltip',
            },
        ],
    },
    {
        title: 'Extra Components',
        icon: 'message-circle-outline',
        children: [
            {
                title: 'Calendar',
                link: '/admin/pages/extra-components/calendar',
            },
            {
                title: 'Progress Bar',
                link: '/admin/pages/extra-components/progress-bar',
            },
            {
                title: 'Spinner',
                link: '/admin/pages/extra-components/spinner',
            },
            {
                title: 'Alert',
                link: '/admin/pages/extra-components/alert',
            },
            {
                title: 'Calendar Kit',
                link: '/admin/pages/extra-components/calendar-kit',
            },
            {
                title: 'Chat',
                link: '/admin/pages/extra-components/chat',
            },
        ],
    },
    {
        title: 'Charts',
        icon: 'pie-chart-outline',
        children: [
            {
                title: 'Echarts',
                link: '/admin/pages/charts/echarts',
            },
            {
                title: 'Charts.js',
                link: '/admin/pages/charts/chartjs',
            },
            {
                title: 'D3',
                link: '/admin/pages/charts/d3',
            },
        ],
    },
    {
        title: 'Editors',
        icon: 'text-outline',
        children: [
            {
                title: 'TinyMCE',
                link: '/admin/pages/editors/tinymce',
            },
            {
                title: 'CKEditor',
                link: '/admin/pages/editors/ckeditor',
            },
        ],
    },
    {
        title: 'Miscellaneous',
        icon: 'shuffle-2-outline',
        children: [
            {
                title: '404',
                link: '/admin/pages/miscellaneous/404',
            },
        ],
    },
   
    {
        title: 'Auth',
        icon: 'lock-outline',
        children: [
            {
                title: 'Login',
                link: '/admin/auth/login',
            },
            {
                title: 'Register',
                link: '/admin/auth/register',
            },
            {
                title: 'Request Password',
                link: '/admin/auth/request-password',
            },
            {
                title: 'Reset Password',
                link: '/admin/auth/reset-password',
            },
        ],
    } */ 
];
