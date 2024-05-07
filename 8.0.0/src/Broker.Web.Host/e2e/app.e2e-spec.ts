import { BrokerTemplatePage } from './app.po';

describe('Broker App', function() {
  let page: BrokerTemplatePage;

  beforeEach(() => {
    page = new BrokerTemplatePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
