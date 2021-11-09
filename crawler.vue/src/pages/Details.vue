<template>
  <div class="container overflow">
    <div class="label row">
      <div class="col">
        <p>Urls(html documents) found after crawling a website: {{ getWebsiteCount() }}</p>
      </div>
      <div class="col">
        <p>Urls found in sitemap: {{ getSitemapCount() }} </p>
      </div>
    </div>
    <div class="accordion" role="tablist">
      <b-card no-body class="mb-1">
        <b-card-header header-tag="header" class="p-0" role="tab">
          <b-button block v-b-toggle.accordion-1 class="w-100 text-white" variant="info">Urls FOUNDED IN SITEMAP.XML but not founded after crawling a web site</b-button>
        </b-card-header>
        <b-collapse id="accordion-1" visible accordion="my-accordion" role="tabpanel">
          <b-card-body>
            <b-card-text>
              <div v-if="onlySitemap.length !== 0">
                <b-table bordered hover :items="onlySitemap" :fields="fields" :sort-by="sortBy" label-sort-clear label-sort-asc label-sort-desc>
                  <template #cell(index)="data">
                    {{ data.index + 1 }}
                  </template>
                  <template #cell(url)="data">
                      <span class="left">{{ data.item.url }}</span>
                  </template>
                  <template #cell(responseTimeMs)="data">
                    <span>{{ data.item.responseTimeMs }}</span>      
                  </template> 
                </b-table>
              </div>
              <div v-else>
                There are no urls
              </div>
            </b-card-text>
          </b-card-body>    
        </b-collapse>
      </b-card>
      <b-card no-body class="mb-1">
        <b-card-header header-tag="header" class="p-0" role="tab">
          <b-button block v-b-toggle.accordion-2 class="w-100 text-white" variant="info">Urls FOUNDED BY CRAWLING THE WEBSITE but not in sitemap.xml</b-button>
        </b-card-header>
        <b-collapse id="accordion-2" accordion="my-accordion" role="tabpanel">
          <b-card-body>
            <b-card-text>
              <div v-if="onlyWebsite.length !== 0">
                <b-table bordered hover responsive="true" :items="onlyWebsite" :fields="fields" :sort-by="sortBy" label-sort-clear label-sort-asc label-sort-desc>
                  <template #cell(index)="data">
                    {{ data.index + 1 }}
                  </template>
                  <template #cell(url)="data">
                      <span class="left">{{ data.item.url }}</span>
                  </template>
                  <template #cell(responseTimeMs)="data">
                    <span>{{ data.item.responseTimeMs }}</span>      
                  </template> 
                </b-table>
                </div>
              <div v-else>
                There are no urls
              </div>
            </b-card-text>
          </b-card-body>
        </b-collapse>
      </b-card>

      <b-card no-body class="mb-1">
        <b-card-header header-tag="header" class="p-0" role="tab">
          <b-button block v-b-toggle.accordion-3 class="w-100 text-white" variant="info">All urls FOUNDED BY CRAWLING THE WEBSITE AND SITEMAP.XML</b-button>
        </b-card-header>
        <b-collapse id="accordion-3" accordion="my-accordion" role="tabpanel">
          <b-card-body>
            <b-card-text>
              <div v-if="onlyWebsite.length !== 0">
                <b-table bordered hover responsive="sm" :items="details" :fields="fields" :sort-by="sortBy" label-sort-clear label-sort-asc label-sort-desc>
                  <template #cell(index)="data">
                    {{ data.index + 1 }}
                  </template>
                  <template #cell(url)="data">
                      <span class="left">{{ data.item.url }}</span>
                  </template>
                  <template #cell(responseTimeMs)="data">
                    <span>{{ data.item.responseTimeMs }}</span>      
                  </template> 
                </b-table>
              </div>
              <div v-else>
                There are no urls
              </div>
            </b-card-text>
          </b-card-body>
        </b-collapse>
      </b-card>
    </div>
    
 
  </div>
</template>

<script>
import axios from "axios";

export default {
  name: "Details",
  data() {
    return {
   
      details: [],
      sortBy : 'responseTimeMs',
      fields: [
        { key: "index", label: "#", sortable: false },
        { key: "url", label: "Url", sortable: true },
        { key: "responseTimeMs", label: "Response Time (Ms)", sortable: true }
      ],
    };
  },
  props: ['testId', 'baseUri'],
  computed:{
    onlySitemap() {
     return this.details.filter(x => !x.inWebsite === true)
    },
    onlyWebsite() {
      return this.details.filter(x => !x.inSitemap === true)
    }   
  },
  methods:{
    getWebsiteCount() {
      return this.details.filter(x => x.inWebsite === true).length
    },
    getSitemapCount() {
      return this.details.filter(x => x.inSitemap === true).length
    }    
  },
  created() {
    axios
      .get(this.baseUri + `/tests/${this.testId}/details`)
      .then((response) => {
        return (this.details = response.data);
      });
  },
  
};
</script>

<style>
.left {
  text-align: left;
  float: left;
}
</style>