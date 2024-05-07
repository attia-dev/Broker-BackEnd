export interface DtCustomSettings extends DataTables.Settings{
    customLanguagePaginateSettings? : CustomLanguagePaginateSettings;
}

interface CustomLanguagePaginateSettings {
    page: string;
    pageOf: string;
}