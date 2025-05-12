//@* <div class="container mt-4"> *@
//@*     <h2 class="text-center mb-4 text-primary fw-bold">Your Cart</h2> *@

//@*     @if (!Model.Any()) *@
//@*     { *@
//@*         <div class="alert alert-info text-center">Your cart is empty!</div> *@
//@*     } *@
//@*     else *@
//@*     { *@
//@*         <div class="row"> *@
//@*             @foreach (var item in Model) *@
//@*             { *@
//@*                 <div class="col-md-6 col-lg-4 mb-4"> *@
//@*                     <div class="card shadow border-0"> *@
//@*                         <div class="row g-0"> *@
//@*                             <div class="col-md-4"> *@
//@*                                 <img src="@item.Product.ImagePath" class="img-fluid rounded-start" alt="@item.Product.ProductName"> *@
//@*                             </div> *@
//@*                             <div class="col-md-8"> *@
//@*                                 <div class="card-body"> *@
//@*                                     <h5 class="card-title text-dark">@item.Product.ProductName</h5> *@
//@*                                     <p class="card-text text-success fw-bold">₹@item.Product.Price</p> *@

//@*                                     <form asp-action="UpdateQuantity" asp-controller="Cart" method="post"> *@
//@*                                         <input type="hidden" name="cartId" value="@item.CartId" /> *@
//@*                                         <label>Quantity:</label> *@
//@*                                         <select name="quantity" class="form-select d-inline w-auto" onchange="this.form.submit()"> *@
//@*                                             @for (int i = 1; i <= 10; i++) *@
//@*                                             { *@
//@*                                                 <option value="@i" selected="@(item.Quantity == i ? "selected" : null)">@i</option> *@
//@*                                             } *@
//@*                                         </select> *@
//@*                                     </form> *@

//@*                                     <a asp-action="RemoveFromCart" asp-route-id="@item.CartId" class="btn btn-danger btn-sm mt-2"> *@
//@*                                         <i class="fas fa-trash"></i> Remove *@
//@*                                     </a> *@
//@*                                 </div> *@
//@*                             </div> *@
//@*                         </div> *@
//@*                     </div> *@
//@*                 </div> *@
//@*             } *@
//@*         </div> *@

//@*         <div class="text-end mt-4"> *@
//@*             <a asp-action="Checkout" class="btn btn-success btn-lg"> *@
//@*                 <i class="fas fa-shopping-cart"></i> Proceed to Checkout *@
//@*             </a> *@
//@*         </div> *@
