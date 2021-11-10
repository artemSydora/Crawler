<template>
   <footer id="footer">
        <div class="container bg-info py-3 bt">
            UKAD &copy; - Crawler - Artem Sydora 2021
        </div>
    </footer>
</template>
<script>
export default {
    name: 'Footer'
}
</script>

<style>
    #footer {
      position: fixed;
      left: 0; bottom: 0;
      padding: 0px;
      color: #fff;
      width: 100%;
      z-index: 4;
   }

   .bt {
     border-top: 10px solid #EAC962;
   }
</style>