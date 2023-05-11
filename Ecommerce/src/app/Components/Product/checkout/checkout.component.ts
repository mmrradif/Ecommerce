import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Router } from '@angular/router';
import { map } from 'rxjs';
import { baseUrl } from 'src/app/Models/Shared/app-constants';
import { UserService } from 'src/app/Services/User/user.service';
import { UserstoreService } from 'src/app/Services/UserStore/userstore.service';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {


  orderForm!: FormGroup;


// For Animation
bubbles!: Array<{ top: string, left: string, size: string, color: string, delay: number }>;



  public fullName$ = this.userstore.getFullNameFromStore().pipe(map(val => val || this.auth.getFullNameFromToken()));


  picPath: string = `${baseUrl}/Pictures`

  cartTotal!: any;
  public products : any = [];

  constructor( private auth : UserService, 
    private userstore :UserstoreService,
    private formBuilder: FormBuilder,
    private snackBar: MatSnackBar,
    private router: Router
    ) { }

  ngOnInit(): void {


    this.orderForm = this.formBuilder.group({
      name: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      address: ['', Validators.required],
      city: ['', Validators.required],
      state: ['', Validators.required],
      zip: ['', Validators.required],
      phone: ['', Validators.required],
    });



    // For Animation
    this.bubbles = [
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '50px', color: '#8f8fff', delay: 0 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '70px', color: '#ff8f8f', delay: 2 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '100px', color: '#8fff8f', delay: 4 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '80px', color: '#ffff8f', delay: 6 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '60px', color: '#ff8fff', delay: 1 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '90px', color: '#8fffff', delay: 3 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '40px', color: '#ff8080', delay: 5 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '70px', color: '#80ff80', delay: 7 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '120px', color: '#8080ff', delay: 8 },
      { top: this.getRandomTop(), left: this.getRandomLeft(), size: '80px', color: '#ffdd80', delay: 9 }
    ];

    

    this.cartTotal =
      JSON.parse(localStorage.getItem('cart_total') as any) || [];
    console.log(this.cartTotal);

    this.products =
      JSON.parse(localStorage.getItem('products') as any) || [];
    console.log(this.products);
  }



  shippingCost: number = 50;

  calculateShippingCost() {
    const shippingMethod = (<HTMLSelectElement>document.getElementById("shipping-method")).value;
    if (shippingMethod === "expedited") {
      this.shippingCost = 70;
    } else {
      this.shippingCost = 50;
    }
  }

  getTotalCost() {
    return this.cartTotal + this.shippingCost;
  }


  placeOrder(): void {
    if (this.orderForm.valid) {
      if (confirm('Are you sure you want to place this order?')) {
        this.snackBar.open('Order confirmed! We are delivered your product very soon.', 'Close', { duration: 3000 });

        this.router.navigate(['product-cart'])
        .then(()=>
        {
          window.location.reload();
        });
       
      }
    }
  }
  
  

  getRandomTop(): string {
    return Math.floor(Math.random() * 70) + '%';
  }
  
  getRandomLeft(): string {
    return Math.floor(Math.random() * 90) + '%';
  }
  
  onAnimationEnd(bubble: any) {
    // update bubble position after animation ends
    bubble.top = this.getRandomTop();
    bubble.left = this.getRandomLeft();
  }
  




}
