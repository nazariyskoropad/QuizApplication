import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestManageAccessComponent } from './test-manage-access.component';

describe('TestManageAccessComponent', () => {
  let component: TestManageAccessComponent;
  let fixture: ComponentFixture<TestManageAccessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TestManageAccessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestManageAccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
