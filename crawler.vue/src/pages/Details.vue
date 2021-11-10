<template>
  <div class="container overflow accordion-margin px-0">
    <div class="label row mb-4">
      <div class="col">
        <p>
          Urls(html documents) found after crawling a website:
          {{ getWebsiteCount() }}
        </p>
      </div>
      <div class="col">
        <p>Urls found in sitemap: {{ getSitemapCount() }}</p>
      </div>
    </div>

    <div class="accordion" role="tablist">

      <accordionItem 
        id="accordion-1" 
        :details="onlySitemap" 
        tableTitle="Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web"
        errorMsg="There are no urls"
      />
      
      <accordionItem 
      id="accordion-2" 
      :details="onlyWebsite" 
      tableTitle="Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml"
      errorMsg="There are no urls"
      />

      <accordionItem 
      id="accordion-3" 
      :details="details" 
      tableTitle="All urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML"
      errorMsg="There are no urls"
      />
    
    </div>
  </div>
</template>

<script>
import axios from "axios";
import AccordionItem from "../components/details/AccordionItem.vue";

export default {
  components: { AccordionItem },
  name: "Details",
  data() {
    return {
      details: [],
    };
  },
  props: ["testId", "baseUri", "userInput"],
  computed: {
    onlySitemap() {
      return this.details.filter((x) => !x.inWebsite === true);
    },
    onlyWebsite() {
      return this.details.filter((x) => !x.inSitemap === true);
    },
  },
  methods: {
    getWebsiteCount() {
      return this.details.filter((x) => x.inWebsite === true).length;
    },
    getSitemapCount() {
      return this.details.filter((x) => x.inSitemap === true).length;
    }   
  },
  created() {
    axios
      .get(this.baseUri + `/tests/${this.testId}/details`)
      .then((response) => {
        return (this.details = response.data);
      });
  }
};
</script>

<style>
.left {
  text-align: left;
  float: left;
}
.accordion-margin {
  margin-bottom: 80px;
}
</style>